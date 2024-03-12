
namespace PetCoffee.Application.Features.Events.Models;

public class EventResponse
{
	public long Id { get; set; }
	public string? Title { get; set; }
	public string? Content { get; set; }
	public string? Image { get; set; }
	public string? Description { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public TimeSpan StartTime { get; set; }
	public TimeSpan EndTime { get; set; }
	public string? Location { get; set; }
	public long PetCoffeeShopId { get; set; }
	public int TotalJoinEvent { get; set; } = 0;
	public bool IsJoin {  get; set; } = false;
	public bool IsCanceled { get; set; } = false;
	public List<FieldEventResponseForEventResponse>? Fields { get; set; }
}
