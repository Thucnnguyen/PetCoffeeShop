
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Events.Models;

public class EventResponse
{
	public long Id { get; set; }
	public string? Title { get; set; }
	public string? Content { get; set; }
	public string? Image { get; set; }
	public string? Description { get; set; }
	public DateTimeOffset StartDate { get; set; }
	public DateTimeOffset EndDate { get; set; }
	public string StartTime { get; set; }
	public string EndTime { get; set; }
	public string? Location { get; set; }
	public long PetCoffeeShopId { get; set; }
	public int TotalJoinEvent { get; set; } = 0;
	public bool IsJoin { get; set; } = false;
	public bool IsCanceled { get; set; } = false;
	public EventStatus Status { get; set; }
	public int MinParticipants { get; set; }
	public int MaxParticipants { get; set; }

	public List<FieldEventResponseForEventResponse>? Fields { get; set; }
}
