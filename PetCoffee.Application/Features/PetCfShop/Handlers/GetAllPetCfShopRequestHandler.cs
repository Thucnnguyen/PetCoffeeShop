using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class GetAllPetCfShopRequestHandler : IRequestHandler<GetAllPetCfShopRequestQuery, PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>>
{

    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public GetAllPetCfShopRequestHandler(
    IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>> Handle(GetAllPetCfShopRequestQuery request,
        CancellationToken cancellationToken)
    {

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
        if (request.Longitude == 0 || request.Latitude == 0)
        {
            response = response.OrderBy(x => x.Distance).ThenBy(x => x.CreatedAt).ToList();
        }
        else
        {
            response = response.OrderByDescending(x => x.CreatedAt).ToList();
        }

        var shopResponses = response
                            .Skip((request.PageNumber - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .ToList();

        return new PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>(
            shopResponses,
            response.Count(),
            request.PageNumber,
            request.PageSize);
    }


}
