using MediatR;
using PetCoffee.Application.Features.Events.Models;

namespace PetCoffee.Application.Features.Events.Queries;

public class GetSubmitEventByIdQuery : IRequest<EventResponse>
{
    public long SubmittingEventId { get; set; }
}
