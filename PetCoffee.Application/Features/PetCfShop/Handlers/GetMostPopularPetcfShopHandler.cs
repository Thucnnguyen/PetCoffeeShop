
using MediatR;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class GetMostPopularPetcfShopHandler : IRequestHandler<GetAllPetCfShopQuery, PaginationResponse<PetCoffeeShop, PetCoffeeShopResponse>>
{
	public Task<PaginationResponse<PetCoffeeShop, PetCoffeeShopResponse>> Handle(GetAllPetCfShopQuery request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
