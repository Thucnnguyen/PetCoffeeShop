

using MediatR;
using PetCoffee.Application.Features.Events.Models;

namespace PetCoffee.Application.Features.Events.Queries;

public class GetSubmittingEventByIdForCustomerQuery : IRequest<EventResponse>
{
    public long EventId { get; set; }
}
