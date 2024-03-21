
using PetCoffee.Application.Common.Models.Response;

namespace PetCoffee.Application.Features.SubmitttingEvents.Models;

public class SubmittingEventResponse : BaseAuditableEntityResponse
{
    public long Id { get; set; }
    public long EventId { get; set; }
    public string? Title { get; set; }
    public string? Image { get; set; }
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Location { get; set; }
    public long PetCoffeeShopId { get; set; }
    public bool IsJoin { get; set; } = true;
    public bool IsCanceled { get; set; } = false;
    public List<EventFieldResponse> EventFields { get; set; }
}