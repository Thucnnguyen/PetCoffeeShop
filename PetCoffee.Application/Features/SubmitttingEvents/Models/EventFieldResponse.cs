

namespace PetCoffee.Application.Features.SubmitttingEvents.Models;

public class EventFieldResponse
{
	public long Id { get; set; }
	public string Question { get; set; }
	public string Type { get; set; }
	public bool IsOptional { get; set; }
	public string? Answer { get; set; }
	public long SubmittinhEventId { get; set; }
	public string SubmmitContent { get; set; }
}
