

namespace PetCoffee.Application.Features.Events.Models;

public class EventForCardResponse
{
	public long Id { get; set; }
	public string? Title { get; set; }
	public string? Image { get; set; }
	public string? Description { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
	public string? Location { get; set; }
	public long TotalJoinEvent { get; set; } = 0;
}
