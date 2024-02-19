
using MediatR;
using PetCoffee.Application.Features.PetCfShop.Models;

namespace PetCoffee.Application.Features.PetCfShop.Queries;

public class GetPetCfShopForCurrentAccountQuery : IRequest<PetCoffeeShopResponse>
{
}
