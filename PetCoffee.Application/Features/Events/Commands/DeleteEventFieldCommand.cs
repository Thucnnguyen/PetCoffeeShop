

using MediatR;

namespace PetCoffee.Application.Features.Events.Commands;

public class DeleteEventFieldCommand : IRequest<bool>
{
    public long EventFieldId { get; set; }
}
