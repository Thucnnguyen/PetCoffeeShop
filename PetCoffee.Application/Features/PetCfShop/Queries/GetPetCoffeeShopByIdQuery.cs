using MediatR;
using PetCoffee.Application.Features.PetCfShop.Models;

namespace PetCoffee.Application.Features.PetCfShop.Queries;

public class GetPetCoffeeShopByIdQuery : IRequest<PetCoffeeShopResponse>
{
	public long Id { get; set; }
}
