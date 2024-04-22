using AutoMapper;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class GetAllPetCfShopRequestHandler : IRequestHandler<GetAllPetCfShopQuery, PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>>
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

	public async Task<PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>> Handle(GetAllPetCfShopQuery request,
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

		var stores = await _unitOfWork.PetCoffeeShopRepository
			.Get(
				predicate: exp,
				disableTracking: true
			)
			.Include(p => p.Areas.Where(a => a.Reservations.Any()))
				.ThenInclude(a => a.Reservations.Where(r => r.Rate != null && r.Status == OrderStatus.Success))
			.Include(p => p.Promotions)
			.Include(p => p.Follows)
			.ToListAsync();

		if (!currentAccount.IsAdmin && !currentAccount.IsPlatformStaff)
		{
			var shopResponseForCus = stores.Select(store =>
			{
				var storeRes = _mapper.Map<PetCoffeeShopForCardResponse>(store);
				storeRes.TotalFollow = store.Follows.Count();
				if (request.Longitude != 0 || request.Latitude != 0)
				{
					storeRes.Distance = CalculateDistanceUltils.CalculateDistance(request.Latitude, request.Longitude, store.Latitude, store.Longitude);
				}
				return storeRes;
			}).ToList();

			shopResponseForCus = shopResponseForCus.OrderBy(x => x.Distance).ThenByDescending(x => x.CreatedAt).ToList();

			var shopResponsesCus = shopResponseForCus
								.Skip((request.PageNumber - 1) * request.PageSize)
								.Take(request.PageSize)
								.ToList();

			return new PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>(
				shopResponsesCus,
				shopResponseForCus.Count(),
				request.PageNumber,
				request.PageSize);
		}

		var shopResponsesForPlatForm = stores
								.Skip((request.PageNumber - 1) * request.PageSize)
								.Take(request.PageSize)
								.Select(s => _mapper.Map<PetCoffeeShopForCardResponse>(s))
								.ToList();

		return new PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>(
				shopResponsesForPlatForm,
				stores.Count(),
				request.PageNumber,
				request.PageSize);
	}


}
