using MediatR;
using PetCoffee.Application.Features.Events.Models;

namespace PetCoffee.Application.Features.Events.Queries;

public class GetEventsByShopIdQuery : IRequest<List<EventForCardResponse>>
{
	public long ShopId { get; set; }
}
