

namespace PetCoffee.Application.Features.Events.Models;

public class EventForCardResponse
{
	public long SubmitEventId { get; set; }
	public long EventId { get; set; }
	public string? Title { get; set; }
	public string? Image { get; set; }
	public string? Description { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public TimeSpan StartTime { get; set; }
	public TimeSpan EndTime { get; set; }
	public string? Location { get; set; }
	public long TotalJoinEvent { get; set; } = 0;
}
