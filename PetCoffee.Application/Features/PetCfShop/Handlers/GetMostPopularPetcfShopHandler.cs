
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class GetMostPopularPetcfShopHandler : IRequestHandler<GetMostPopularPetcfShopQuery, PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>>
{


	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public GetMostPopularPetcfShopHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>> Handle(GetMostPopularPetcfShopQuery request, CancellationToken cancellationToken)
	{
		var CurrentUser = await _currentAccountService.GetCurrentAccount();
		if (CurrentUser == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (CurrentUser.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}

		var stores = (await _unitOfWork.PetCoffeeShopRepository.GetAsync(
			predicate: request.GetExpressions(),
			disableTracking: true
		)).ToList();
		var response = new List<PetCoffeeShopForCardResponse>();

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
		response = response.OrderBy(x => x.TotalFollow).ThenBy(x => x.CreatedAt).ToList();
		response = response
			   .Skip((request.PageNumber - 1) * request.PageSize)
			   .Take(request.PageSize)
			   .ToList();
		return new PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>(
			response,
			stores.Count(),
			request.PageNumber,
			request.PageSize);
	}

	
}
