using AutoMapper;
using MediatR;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class GetRandomPetCfShopHandler : IRequestHandler<GetRandomPetCfShopQuery, IList<PetCoffeeShopForCardResponse>>
{
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
		if (request.ShopType != null)
		{
			TotalShop = await _unitOfWork.PetCoffeeShopRepository.CountAsync(s => s.Type == request.ShopType && s.Status == ShopStatus.Active && s.IsBuyPackage);
		}
		else
		{
			TotalShop = await _unitOfWork.PetCoffeeShopRepository.CountAsync();
		}
		var response = new List<PetCoffeeShopForCardResponse>();
		var stores = (await _unitOfWork.PetCoffeeShopRepository.GetAsync(s => s.Status == ShopStatus.Active,
					includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.PetCoffeeShop, object>>>()
					{
						ps => ps.Promotions,
					}
			)).ToList();
								

		if (TotalShop < request.Size)
		{
			foreach (var store in stores)
			{
				var storeRes = _mapper.Map<PetCoffeeShopForCardResponse>(store);
				storeRes.Distance = CalculateDistanceUltils.CalculateDistance(request.Latitude, request.Longitude, store.Latitude, store.Longitude);
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
			storeRes.Distance = CalculateDistanceUltils.CalculateDistance(request.Latitude, request.Longitude, store.Latitude, store.Longitude);
			storeRes.TotalFollow = await _unitOfWork.FollowPetCfShopRepository.CountAsync(f => f.ShopId == store.Id);
			response.Add(storeRes);
		}
		return response;

	}
}
