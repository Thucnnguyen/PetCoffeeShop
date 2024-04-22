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
using PetCoffee.Domain.Entities;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Post.Handlers
{
	public class GetPostOFShopBeTaggedHandler : IRequestHandler<GetPostOFShopBeTaggedQuery, PaginationResponse<Domain.Entities.Post, PostResponse>>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentAccountService _currentAccountService;
		public GetPostOFShopBeTaggedHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_currentAccountService = currentAccountService;
		}

		public async Task<PaginationResponse<Domain.Entities.Post, PostResponse>> Handle(GetPostOFShopBeTaggedQuery request, CancellationToken cancellationToken)
		{
			var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
			if (currentAccount == null)
			{
				throw new ApiException(ResponseCode.AccountNotExist);
			}
			if (currentAccount.IsVerify)
			{
				throw new ApiException(ResponseCode.AccountNotActived);
			}

			var CurrentShop = (await _unitOfWork.PetCoffeeShopRepository.GetAsync(
		predicate: p => p.Id == request.ShopId && !p.Deleted,
		includes: new List<Expression<Func<PetCoffeeShop, object>>>()
			{
					shop => shop.CreatedBy
			},
		disableTracking: true
		)).FirstOrDefault();

			if (CurrentShop == null)
			{
				throw new ApiException(ResponseCode.ShopNotExisted);
			}


			//var postPetCfShop = await _unitOfWork.PostCoffeeShopRepository.GetAsync(pc => pc.ShopId == CurrentShop.Id);

			//var postPetCfShop = await _unitOfWork.PostCoffeeShopRepository.Get(
			//					predicate: p => p.ShopId == request.ShopId && !p.Deleted,
			//					disableTracking: true
			//				)
			//				.Include(p => p.Post)
								
											
			//				.ToListAsync();
			var postPetCfShop = _unitOfWork.PostRepository.Get(
			  predicate: p => p.PostPetCoffeeShops.Any(p => p.ShopId == request.ShopId && !p.Deleted),
			  disableTracking: true
			)
			.Include(p => p.Likes)
			.Include(p => p.PetCoffeeShop)
			.Include(p => p.Comments)
			.Include(p => p.PostCategories)
			.ThenInclude(c => c.Category)
			.Include(p => p.PostPetCoffeeShops)
			.ThenInclude(shop => shop.Shop)
			.Include(p => p.CreatedBy)
			.AsQueryable();

			var response = new List<PostResponse>();

			foreach (var post in postPetCfShop)
			{
				var postResponse = _mapper.Map<PostResponse>(post);
				postResponse.TotalComment = post.Comments.Count();
				postResponse.TotalLike = post.Likes.Count();
				postResponse.IsLiked = post.Likes.FirstOrDefault(l => l.CreatedById == currentAccount.Id) != null;


				response.Add(postResponse);
			}

			response = response
		   .Skip((request.PageNumber - 1) * request.PageSize)
		   .Take(request.PageSize)
		   .ToList();

			return new PaginationResponse<Domain.Entities.Post, PostResponse>(
				response,
				postPetCfShop.Count(),
				request.PageNumber,
				request.PageSize);

		}
	}
}
