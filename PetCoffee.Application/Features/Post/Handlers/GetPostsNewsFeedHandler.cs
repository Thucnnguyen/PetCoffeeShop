using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Post.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
            ).Include(p => p.Comments)
			.ThenInclude(com => com.CreatedBy)
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

            
            var response = await postsQuery
                .AsNoTracking()
                .Select(post => _mapper.Map<PostResponse>(post))
                .ToListAsync();

          

            
            return new PaginationResponse<Domain.Entities.Post, PostResponse>(
                response,
                response.Count(),
                request.PageNumber,
                request.PageSize);
        }
    }
}
