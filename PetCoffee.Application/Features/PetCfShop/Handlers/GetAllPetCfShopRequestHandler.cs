using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;

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
		
		var stores = await _unitOfWork.PetCoffeeShopRepository.GetAsync(
			predicate: request.GetExpressions(),
			disableTracking: true
		);
		var response = new List<PetCoffeeShopForCardResponse>();

        return new PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>(
            stores,
            request.PageNumber,
            request.PageSize,
            s => _mapper.Map<PetCoffeeShopForCardResponse>(s));
    }

    
}
