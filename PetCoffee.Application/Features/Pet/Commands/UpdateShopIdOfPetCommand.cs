
using MediatR;

namespace PetCoffee.Application.Features.Pet.Commands;

public class UpdateShopIdOfPetCommand : IRequest<bool>
{
	public long PetId { get; set; }
	public long ShopId { get; set;}
}
