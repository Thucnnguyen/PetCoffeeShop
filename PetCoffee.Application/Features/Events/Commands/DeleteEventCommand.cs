
using MediatR;

namespace PetCoffee.Application.Features.Events.Commands;

public class DeleteEventCommand : IRequest<bool>
{
    public long EventId { get; set; }
}
