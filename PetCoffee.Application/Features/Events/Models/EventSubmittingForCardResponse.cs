﻿

using PetCoffee.Application.Features.Post.Models;

namespace PetCoffee.Application.Features.Events.Models;

public class EventSubmittingForCardResponse
{
	public long SubmitEventId { get; set; }
	public long EventId { get; set; }
	public string? Title { get; set; }
	public string? Image { get; set; }
	public string? Description { get; set; }
	public DateTimeOffset StartDate { get; set; }
	public DateTimeOffset EndDate { get; set; }
	public string StartTime { get; set; }
	public string EndTime { get; set; }
	public string? Location { get; set; }
	public long TotalJoinEvent { get; set; } = 0;
	public AccountForPostModel? AccountForPostModel { get; set; }
}
