
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Features.Post.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class GetPetCofffeeShopByIdHandler : IRequestHandler<GetPetCoffeeShopByIdQuery, PetCoffeeShopResponse>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _accountService;

	public GetPetCofffeeShopByIdHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService accountService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_accountService = accountService;
	}

	public async Task<PetCoffeeShopResponse> Handle(GetPetCoffeeShopByIdQuery request, CancellationToken cancellationToken)
	{
		var CurrentUser = await _accountService.GetCurrentAccount();
		if (CurrentUser == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (CurrentUser.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}

		var CurrentShop = await _unitOfWork.PetCoffeeShopRepository.Get(
			predicate: p => p.Id == request.Id && !p.Deleted && p.Status == ShopStatus.Active,
			includes: new List<Expression<Func<PetCoffeeShop, object>>>()
				{
					shop => shop.CreatedBy,
					shop => shop.Products,
					shop => shop.Areas
				},
			disableTracking: true
			).FirstOrDefaultAsync();

		if (CurrentShop == null)
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}
		
		// inital response
		var response = _mapper.Map<PetCoffeeShopResponse>(CurrentShop);
		response.CreatedBy = _mapper.Map<AccountForPostModel>(CurrentShop.CreatedBy);

		//get max seat
		var maxSeat = CurrentShop.Areas.MaxBy(a => a.TotalSeat);

		response.MaxSeat = maxSeat != null ? maxSeat.TotalSeat : 0;

		// min - max price  product 
		var maxPriceProduct = CurrentShop.Products.MaxBy(p => p.Price);
		var minPriceProduct = CurrentShop.Products.MinBy(p => p.Price);

		response.MaxPriceProduct = maxPriceProduct != null ? maxPriceProduct.Price : 0;
		response.MinPriceProduct = minPriceProduct != null ? minPriceProduct.Price : 0;

		// get min- max Area price of shop
		var areaWithMaxPrice = CurrentShop.Areas.MaxBy(a => a.PricePerHour);
		var areaWithMinPrice = CurrentShop.Areas.MinBy(a => a.PricePerHour);

		response.MaxPriceArea = areaWithMaxPrice != null ? areaWithMaxPrice.PricePerHour: 0;
		response.MinPriceProduct = areaWithMinPrice != null ? areaWithMinPrice.PricePerHour: 0;


		response.StartTime = CurrentShop.OpeningTime;
		response.EndTime = CurrentShop.ClosedTime;

		response.TotalFollow = await _unitOfWork.FollowPetCfShopRepository.CountAsync(f => f.ShopId == request.Id);
		response.IsFollow = (await _unitOfWork.FollowPetCfShopRepository.GetAsync(s => s.CreatedById == CurrentUser.Id && s.ShopId == CurrentShop.Id)).Any();
		// calculate distance
		if (request.Longitude != 0 && request.Latitude != 0)
		{
			response.Distance = CalculateDistanceUltils.CalculateDistance(request.Latitude, request.Longitude, CurrentShop.Latitude, CurrentShop.Longitude);
		}
		return response;
	}
}
