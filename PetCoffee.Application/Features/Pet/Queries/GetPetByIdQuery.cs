
using MediatR;
using PetCoffee.Application.Features.Pet.Models;

namespace PetCoffee.Application.Features.Pet.Queries;

public class GetPetByIdQuery : IRequest<PetResponse>
{
	public long Id { get; set; }
}
