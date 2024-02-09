
using MediatR;
using PetCoffee.Application.Features.Pet.Models;

namespace PetCoffee.Application.Features.Pet.Queries;

public class GetPetsByShopIdQuery : IRequest<IList<PetResponse>>
{
	public long ShopId { get; set; }
}
