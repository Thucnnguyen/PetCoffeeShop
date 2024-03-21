
using MediatR;
using PetCoffee.Application.Features.Events.Models;

namespace PetCoffee.Application.Features.Events.Commands;

public class CreateEventFieldCommand : IRequest<List<FieldEventResponseForEventResponse>>
{
    public long EventId { get; set; }
    public List<CreateFieldEvent> Fields { get; set; }
}

public class CreateFieldEvent
{
    public string Question { get; set; }
    public string Type { get; set; }
    public bool IsOptional { get; set; }
    public string? Answer { get; set; }
}