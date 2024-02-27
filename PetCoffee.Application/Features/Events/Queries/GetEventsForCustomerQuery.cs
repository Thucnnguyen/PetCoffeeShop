using MediatR;
using PetCoffee.Application.Features.Events.Models;

namespace PetCoffee.Application.Features.Events.Queries;

public class GetEventsForCustomerQuery : IRequest<List<EventResponse>>
{
}
