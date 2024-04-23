
using MediatR;
using PetCoffee.Application.Features.Events.Models;

namespace PetCoffee.Application.Features.Events.Commands;

public class UpdateEventFieldsCommand : IRequest<List<FieldEventResponseForEventResponse>>
{
	public long EventId { get; set; }
	public List<CreateFieldEvent> Fields { get; set; }
}

