
using MediatR;

namespace PetCoffee.Application.Features.Pet.Commands;

public class UpdatePetAreaCommand : IRequest<bool>
{
	public long AreaId { get; set; }
	public List<long> PetIds { get; set; }
}
