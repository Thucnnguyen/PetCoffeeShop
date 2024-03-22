

namespace PetCoffee.Application.Features.Events.Models;

public class EventForCardResponse
{
	public long EventId { get; set; }
	public string? Title { get; set; }
	public string? Image { get; set; }
	public string? Description { get; set; }
	public DateTimeOffset StartDate { get; set; }
	public DateTimeOffset EndDate { get; set; }
	public string StartTime { get; set; }
	public string EndTime { get; set; }
	public string? Location { get; set; }
	public bool IsJoin { get; set; }
	public long TotalJoinEvent { get; set; } = 0;
}
