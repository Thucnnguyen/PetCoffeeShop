
using MediatR;
using PetCoffee.Application.Features.Events.Models;

namespace PetCoffee.Application.Features.Events.Queries;

public class GetAllSubmittingEventByEventIdForShopQuery : IRequest<EventResponse>
{
	public long EventId { get; set; }
}
