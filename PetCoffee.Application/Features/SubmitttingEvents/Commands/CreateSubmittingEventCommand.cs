
using MediatR;
using PetCoffee.Application.Features.SubmitttingEvents.Models;

namespace PetCoffee.Application.Features.SubmitttingEvents.Commands;

public class CreateSubmittingEventCommand : IRequest<SubmittingEventResponse>
{
	public long EventId { get; set; }
	public List<CreateSubmittingEventField>? Answers { get; set; }
}

public class CreateSubmittingEventField
{
	public long EventFieldId { get; set; }
	public string? Submitcontent { get; set; }
}