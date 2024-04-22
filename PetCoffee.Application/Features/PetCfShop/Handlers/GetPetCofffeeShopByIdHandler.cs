
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

		PetCoffeeShop CurrentShop;
		if (CurrentUser.IsAdmin)
		{
			CurrentShop = await _unitOfWork.PetCoffeeShopRepository.Get(
					predicate: p => p.Id == request.Id && !p.Deleted,
					includes: new List<Expression<Func<PetCoffeeShop, object>>>()
						{
							shop => shop.CreatedBy,
							//shop => shop.Products,
							shop => shop.Areas,
							shop => shop.Follows,
							shop => shop.Promotions,
						},
					disableTracking: true
					).FirstOrDefaultAsync();
		}
		else
		{
			CurrentShop = await _unitOfWork.PetCoffeeShopRepository.Get(
					predicate: p => p.Id == request.Id && !p.Deleted && p.Status == ShopStatus.Active,
					disableTracking: true
					)
					.Include(shop => shop.CreatedBy)
					//.Include(shop => shop.Products.Where(p => !p.Deleted))
					.Include(shop => shop.Areas.Where(a => !a.Deleted))
					.Include(shop => shop.Follows)
					.Include(shop => shop.Promotions)
					.FirstOrDefaultAsync();
		}

					


		if (CurrentShop == null)
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}

		// inital response
		var response = _mapper.Map<PetCoffeeShopResponse>(CurrentShop);
		response.CreatedBy = _mapper.Map<AccountForPostModel>(CurrentShop.CreatedBy);
		//Count Rate
		if (!CurrentUser.IsAdmin)
		{
			var reservations = await _unitOfWork.ReservationRepository.Get(r => r.Status == OrderStatus.Overtime && r.Area.PetcoffeeShopId == CurrentShop.Id && r.Rate != null)
							.ToListAsync();
			response.Rates = reservations.Average(r => r.Rate);
		}
		
		//get max seat
		var maxSeat = CurrentShop.Areas.MaxBy(a => a.TotalSeat);

		response.MaxSeat = maxSeat != null ? maxSeat.TotalSeat : 0;

		// min - max price  product 
		//var maxPriceProduct = CurrentShop.Products.MaxBy(p => p.Price);
		//var minPriceProduct = CurrentShop.Products.MinBy(p => p.Price);

		//response.MaxPriceProduct = maxPriceProduct != null ? maxPriceProduct.Price : 0;
		//response.MinPriceProduct = minPriceProduct != null ? minPriceProduct.Price : 0;

		// get min- max Area price of shop
		var areaWithMaxPrice = CurrentShop.Areas.MaxBy(a => a.PricePerHour);
		var areaWithMinPrice = CurrentShop.Areas.MinBy(a => a.PricePerHour);

		response.MaxPriceArea = areaWithMaxPrice != null ? areaWithMaxPrice.PricePerHour : 0;
		response.MinPriceArea = areaWithMinPrice != null ? areaWithMinPrice.PricePerHour : 0;


		response.StartTime = CurrentShop.OpeningTime;
		response.EndTime = CurrentShop.ClosedTime;

		response.TotalFollow = CurrentShop.Follows.Count();
		response.IsFollow = CurrentShop.Follows.Any(f => f.CreatedById == CurrentUser.Id && f.ShopId == CurrentShop.Id);

		// calculate distance
		if (request.Longitude != 0 && request.Latitude != 0)
		{
			response.Distance = CalculateDistanceUltils.CalculateDistance(request.Latitude, request.Longitude, CurrentShop.Latitude, CurrentShop.Longitude);
		}
		return response;
	}
}
