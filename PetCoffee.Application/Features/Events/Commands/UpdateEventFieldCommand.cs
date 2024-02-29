

using MediatR;
using PetCoffee.Application.Features.SubmitttingEvents.Models;

namespace PetCoffee.Application.Features.Events.Commands;

public class UpdateEventFieldCommand : IRequest<EventFieldResponse>
{
	public string Id { get; set; }
	public string FieldName { get; set; }
	public string FieldValue { get; set; }
	public string? OptionValue { get; set; }
	public bool IsOptional { get; set; }
	public string? Answer { get; set; }
	public int Order { get; set; }
}
