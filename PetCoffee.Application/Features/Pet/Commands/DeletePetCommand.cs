

using MediatR;

namespace PetCoffee.Application.Features.Pet.Commands;

public class DeletePetCommand : IRequest<bool>
{
    public long Id { get; set; }
}
