
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Features.Post.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
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
		var CurrentShop = (await _unitOfWork.PetCoffeeShopRepository.GetAsync(
			predicate: p => p.Id == request.Id && !p.Deleted,
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
		// get maxSeat of shop
		var areasWithMaxSeat = await _unitOfWork.AreaRepsitory
		.GetAsync(
			predicate: a => a.PetcoffeeShopId == CurrentShop.Id, // Filter by shop ID
			orderBy: q => q.OrderByDescending(a => a.TotalSeat), // Order by TotalSeat in descending order
			disableTracking: true
		);

		var seat = areasWithMaxSeat.FirstOrDefault();

		int areasWithMaxSeatOfShop = 0;
		if (seat == null)
		{
			areasWithMaxSeatOfShop = 0;
		}
		else
		{

			areasWithMaxSeatOfShop = areasWithMaxSeat.First().TotalSeat;
		}
		var response = _mapper.Map<PetCoffeeShopResponse>(CurrentShop);


		if (request.Longitude != 0 && request.Latitude != 0)
		{
			response.Distance = CalculateDistanceUltils.CalculateDistance(request.Latitude, request.Longitude, CurrentShop.Latitude, CurrentShop.Longitude);
		}
		response.TotalFollow = await _unitOfWork.FollowPetCfShopRepository.CountAsync(f => f.ShopId == request.Id);
		response.IsFollow = (await _unitOfWork.FollowPetCfShopRepository.GetAsync(s => s.CreatedById == CurrentUser.Id && s.ShopId == CurrentShop.Id)).Any();
		response.CreatedBy = _mapper.Map<AccountForPostModel>(CurrentShop.CreatedBy);
		response.MaxSeat = areasWithMaxSeatOfShop;

		// get set price 
		// max price  product 

		// get maxSeat of shop
		var productWithMaxPrice = await _unitOfWork.ProductRepository
		.GetAsync(
			predicate: a => a.PetCoffeeShopId == CurrentShop.Id && a.ProductStatus == Domain.Enums.ProductStatus.Active, // Filter by shop ID
			orderBy: q => q.OrderByDescending(a => a.Price), // Order by TotalSeat in descending order
			disableTracking: true
		);

		var proMaxPrice = productWithMaxPrice.FirstOrDefault();

		decimal productWithMaxPriceOfShops = 0;
		if (proMaxPrice == null)
		{
			productWithMaxPriceOfShops = 0;
		}
		else
		{

			productWithMaxPriceOfShops = productWithMaxPrice.First().Price;
		}

		response.MaxPriceProduct = productWithMaxPriceOfShops;

		// min price  product 

		var productWithMinPrice = await _unitOfWork.ProductRepository
		.GetAsync(
			predicate: a => a.PetCoffeeShopId == CurrentShop.Id && a.ProductStatus == Domain.Enums.ProductStatus.Active, // Filter by shop ID
			orderBy: q => q.OrderBy(a => a.Price), // Order by TotalSeat in descending order
			disableTracking: true
		);

		var proMinPrice = productWithMinPrice.FirstOrDefault();

		decimal productWithMinPriceOfShops = 0;
		if (proMinPrice == null)
		{
			productWithMinPriceOfShops = 0;
		}
		else
		{

			productWithMinPriceOfShops = productWithMinPrice.First().Price;
		}

		response.MinPriceProduct = productWithMinPriceOfShops;


		// get maxArea price of shop
		var areaWithMaxPrice = await _unitOfWork.AreaRepsitory
		.GetAsync(
			predicate: a => a.PetcoffeeShopId == CurrentShop.Id && !a.Deleted, // Filter by shop ID
			orderBy: q => q.OrderByDescending(a => a.PricePerHour), // Order by TotalSeat in descending order
			disableTracking: true
		);

		var areaMaxPrice = areaWithMaxPrice.FirstOrDefault();

		decimal areaWithMaxPriceOfShops = 0;
		if (areaMaxPrice == null)
		{
			areaWithMaxPriceOfShops = 0;
		}
		else
		{

			areaWithMaxPriceOfShops = areaWithMaxPrice.First().PricePerHour;
		}

		response.MaxPriceArea = areaWithMaxPriceOfShops;

		// get minArea price of shop
		var areaWithMinPrice = await _unitOfWork.AreaRepsitory
		.GetAsync(
			predicate: a => a.PetcoffeeShopId == CurrentShop.Id && !a.Deleted, // Filter by shop ID
			orderBy: q => q.OrderBy(a => a.PricePerHour), // Order by TotalSeat in descending order
			disableTracking: true
		);

		var areaMinPrice = areaWithMinPrice.FirstOrDefault();

		decimal areaWithMinPriceOfShops = 0;
		if (areaMinPrice == null)
		{
			areaWithMinPriceOfShops = 0;
		}
		else
		{

			areaWithMinPriceOfShops = areaWithMinPrice.First().PricePerHour;
		}

		response.MinPriceArea = areaWithMinPriceOfShops;


		response.StartTime = DateTimeOffset.TryParse(CurrentShop.OpeningTime, out DateTimeOffset startTime) ? startTime : null;





		response.EndTime = DateTimeOffset.TryParse(CurrentShop.ClosedTime, out DateTimeOffset closeTime) ? closeTime : null;








		return response;
	}
}
