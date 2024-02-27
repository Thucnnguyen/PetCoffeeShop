
namespace PetCoffee.Application.Features.Events.Models;

public class EventResponse
{
	public long Id { get; set; }
	public string? Title { get; set; }
	public string? Content { get; set; }
	public string? Image { get; set; }
	public string? Description { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
	public string? Location { get; set; }
	public long PetCoffeeShopId { get; set; }
	public int TotalSubmitEvent { get; set; } = 0;
	public List<FieldEventResponseForEventResponse>? Fields { get; set; }
}
