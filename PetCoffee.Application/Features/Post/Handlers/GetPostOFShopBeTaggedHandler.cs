using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Post.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Shared.Ultils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

			var postPetCfShop = (await _unitOfWork.PostCoffeeShopRepository.GetAsync(
	predicate: p => p.ShopId == request.ShopId && !p.Deleted,
	includes: new List<Expression<Func<PostPetCoffeeShop, object>>>()
		{
					shop => shop.Post
		},
	disableTracking: true
	)).ToList();

			var response = new List<PostResponse>();

			foreach (var postTag in  postPetCfShop)
			{
				var postTagRes = _mapper.Map<PostResponse>(postTag.Post);
			

				response.Add(postTagRes);
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
