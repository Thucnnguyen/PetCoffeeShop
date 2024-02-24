using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using System.Security.Cryptography.X509Certificates;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class GetAllPetCfShopHandler : IRequestHandler<GetAllPetCfShopQuery, PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>>
{
	private const Double RADIUS = 6378.16;
	private const double PI = Math.PI / 180;

	private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public GetAllPetCfShopHandler(
    IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>> Handle(GetAllPetCfShopQuery request,
        CancellationToken cancellationToken)
    {
		
		var stores =(await _unitOfWork.PetCoffeeShopRepository.GetAsync(
			predicate: request.GetExpressions(),
			disableTracking: true
		)).ToList();

		var response = new List<PetCoffeeShopForCardResponse>();

		if(request.Longitude == 0 || request.Latitude == 0)
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
				storeRes.Distance = CalculateDistance(request.Latitude, request.Longitude, store.Latitude, store.Longitude);
				storeRes.TotalFollow = storeRes.TotalFollow = await _unitOfWork.FollowPetCfShopRepository.CountAsync(f => f.ShopId == store.Id);

				response.Add(storeRes);
			}
		}
		if(request.Longitude == 0 || request.Latitude == 0)
		{
			response = response.OrderBy(x => x.Distance).ThenBy(x => x.CreatedAt).ToList();
		}
			
		response = response.OrderBy(x => x.CreatedAt).ToList();

		return new PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>(
			response,
			response.Count(),
            request.PageNumber,
            request.PageSize);
    }

    private double CalculateDistance(double userLatitude,double userLongitude, double ShopLatitude,double ShopLongitude)
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
