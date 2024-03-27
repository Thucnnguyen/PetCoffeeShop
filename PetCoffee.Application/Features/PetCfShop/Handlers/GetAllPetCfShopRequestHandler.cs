using AutoMapper;
using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class GetAllPetCfShopRequestHandler : IRequestHandler<GetAllPetCfShopRequestQuery, PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>>
{

	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public GetAllPetCfShopRequestHandler(
	IUnitOfWork unitOfWork,
		IMapper mapper,
		ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_currentAccountService = currentAccountService;
	}

	public async Task<PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>> Handle(GetAllPetCfShopRequestQuery request,
		CancellationToken cancellationToken)
	{
		var currentAccount = await _currentAccountService.GetCurrentAccount();
		if (currentAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (currentAccount.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}
		var exp = request.GetExpressions();
		if (currentAccount.IsAdmin || currentAccount.IsPlatformStaff)
		{
			exp = exp.And(s => !s.Deleted);
		}
		else
		{
			exp = exp.And(shop => shop.Status == ShopStatus.Active && shop.IsBuyPackage && !shop.Deleted);
		}

		var stores = (await _unitOfWork.PetCoffeeShopRepository.GetAsync(
			predicate: exp,
			disableTracking: true
		)).ToList();

		var response = new List<PetCoffeeShopForCardResponse>();
		if (!currentAccount.IsAdmin && !currentAccount.IsPlatformStaff)
		{
			if (request.Longitude == 0 || request.Latitude == 0)
			{
				foreach (var store in stores)
				{
					var storeRes = _mapper.Map<PetCoffeeShopForCardResponse>(store);
					storeRes.TotalFollow = await _unitOfWork.FollowPetCfShopRepository.CountAsync(f => f.ShopId == store.Id);
					response.Add(storeRes);
				}
			}
			else
			{
				foreach (var store in stores)
				{
					var storeRes = _mapper.Map<PetCoffeeShopForCardResponse>(store);
					storeRes.Distance = CalculateDistanceUltils.CalculateDistance(request.Latitude, request.Longitude, store.Latitude, store.Longitude);
					storeRes.TotalFollow = storeRes.TotalFollow = await _unitOfWork.FollowPetCfShopRepository.CountAsync(f => f.ShopId == store.Id);

					response.Add(storeRes);
				}
			}
			if (request.Longitude == 0 || request.Latitude == 0)
			{
				response = response.OrderBy(x => x.Distance).ThenBy(x => x.CreatedAt).ToList();
			}
			else
			{
				response = response.OrderByDescending(x => x.CreatedAt).ToList();
			}

			var shopResponsesCus = response
								.Skip((request.PageNumber - 1) * request.PageSize)
								.Take(request.PageSize)
								.ToList();
			return new PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>(
				shopResponsesCus,
				response.Count(),
				request.PageNumber,
				request.PageSize);
		}

		var shopResponses = stores
								.Skip((request.PageNumber - 1) * request.PageSize)
								.Take(request.PageSize)
								.Select(s => _mapper.Map<PetCoffeeShopForCardResponse>(s))
								.ToList();

		return new PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>(
				shopResponses,
				stores.Count(),
				request.PageNumber,
				request.PageSize);
	}


}
