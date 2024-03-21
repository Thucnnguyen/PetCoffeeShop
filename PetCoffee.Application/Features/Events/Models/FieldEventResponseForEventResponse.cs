
namespace PetCoffee.Application.Features.Events.Models;

public class FieldEventResponseForEventResponse
{
    public long Id { get; set; }
    public string Question { get; set; }
    public string Type { get; set; }
    public bool IsOptional { get; set; }
    public string? Answer { get; set; }
    public long EventId { get; set; }
    public string SubmmitContent { get; set; }

}
