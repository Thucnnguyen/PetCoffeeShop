using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Post.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System.Security.Cryptography.X509Certificates;

namespace PetCoffee.Application.Features.Post.Handlers
{
    public class GetPostsNewsFeedHandler : IRequestHandler<GetPostsNewsFeedQuery, PaginationResponse<Domain.Entities.Post, PostResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentAccountService _currentAccountService;
        public GetPostsNewsFeedHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentAccountService = currentAccountService;
        }

        public async Task<PaginationResponse<Domain.Entities.Post, PostResponse>> Handle(GetPostsNewsFeedQuery request, CancellationToken cancellationToken)
        {

         
            var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
            if (currentAccount == null)
            {
                throw new ApiException(ResponseCode.AccountNotExist);
            }

            
            var followedShopIds = (await _unitOfWork.FollowPetCfShopRepository.GetAsync(f => f.CreatedById == currentAccount.Id)).ToList();
         
            var postsQuery =  _unitOfWork.PostRepository.Get(
              predicate: request.GetExpressions(),
              disableTracking: true
            )
            .Include(p => p.Likes)
            .Include(p => p.Comments)
			.Include(p => p.PostCategories)
			.ThenInclude(c => c.Category)
			.Include(p => p.PostPetCoffeeShops)
			.ThenInclude(shop => shop.Shop)
			.Include(p => p.CreatedBy).AsQueryable();
            
            if (followedShopIds.Any())
            {
                postsQuery = postsQuery
          
          .OrderByDescending(post => post.PostPetCoffeeShops.Any(pcs => followedShopIds.Select(f => f.ShopId).Contains(pcs.ShopId)))
          .ThenByDescending(post => post.Likes.Count) 
          .ThenByDescending(post => post.CreatedAt); 
            }
            else
            {
                
                postsQuery = postsQuery
                    .OrderByDescending(post => post.Likes.Count)
                    .ThenByDescending(post => post.CreatedAt);
            }

            
            var response = new List<PostResponse>();
            foreach ( var post in postsQuery.ToList())
            {
                var postResponse = _mapper.Map<PostResponse>( post );
                postResponse.TotalComment = post.Comments.Count();
                postResponse.TotalLike = post.Likes.Count();
                postResponse.IsLiked = (await _unitOfWork.LikeRepository.GetAsync(l => l.PostId == post.Id && l.CreatedById == currentAccount.Id)).Any();
                response.Add( postResponse );
            }
            
            return new PaginationResponse<Domain.Entities.Post, PostResponse>(
                response,
                response.Count(),
                request.PageNumber,
                request.PageSize);
        }
    }
}
