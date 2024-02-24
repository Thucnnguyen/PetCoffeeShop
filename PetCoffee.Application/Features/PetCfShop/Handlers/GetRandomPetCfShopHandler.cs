

using AutoMapper;
using MediatR;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Persistence.Repository;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class GetRandomPetCfShopHandler : IRequestHandler<GetRandomPetCfShopQuery, IList<PetCoffeeShopForCardResponse>>
{
	private const Double RADIUS = 6378.16;
	private const double PI = Math.PI / 180;

	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public GetRandomPetCfShopHandler(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task<IList<PetCoffeeShopForCardResponse>> Handle(GetRandomPetCfShopQuery request, CancellationToken cancellationToken)
	{
		int TotalShop = 0;
		if(request.ShopType != null)
		{
			TotalShop = await _unitOfWork.PetCoffeeShopRepository.CountAsync(s =>s.Type == request.ShopType);
		}
		else
		{
			TotalShop = await _unitOfWork.PetCoffeeShopRepository.CountAsync();
		}
		var response = new List<PetCoffeeShopForCardResponse>();
		var stores = (await _unitOfWork.PetCoffeeShopRepository.GetAsync()).ToList();

		if (TotalShop < request.Size)
		{
			foreach (var store in stores)
			{
				var storeRes = _mapper.Map<PetCoffeeShopForCardResponse>(store);
				storeRes.Distance = CalculateDistance(request.Latitude, request.Longitude, store.Latitude, store.Longitude);
				storeRes.TotalFollow = await _unitOfWork.FollowPetCfShopRepository.CountAsync(f => f.ShopId == store.Id);
				response.Add(storeRes);
			}
			return response;
		}

		int skip = new Random().Next(0, TotalShop - request.Size);
		var randomShop = stores.Skip(skip)
						   .Take(request.Size)
						   .ToList();

		foreach (var store in randomShop)
		{
			var storeRes = _mapper.Map<PetCoffeeShopForCardResponse>(store);
			storeRes.Distance = CalculateDistance(request.Latitude, request.Longitude, store.Latitude, store.Longitude);
			storeRes.TotalFollow = await _unitOfWork.FollowPetCfShopRepository.CountAsync(f => f.ShopId == store.Id);
			response.Add(storeRes);
		}
		return response;

	}
	private double CalculateDistance(double userLatitude, double userLongitude, double ShopLatitude, double ShopLongitude)
	{
		double dlon = Radians(ShopLongitude - userLongitude);
		double dlat = Radians(ShopLatitude - userLatitude);

		double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(userLatitude)) * Math.Cos(Radians(ShopLatitude)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
		double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
		return angle * RADIUS;
	}
	/// <summary>
	/// Convert degrees to Radians
	/// </summary>
	/// <param name="x">Degrees</param>
	/// <returns>The equivalent in radians</returns>
	public static double Radians(double x)
	{
		return x * PI / 180;
	}
}
