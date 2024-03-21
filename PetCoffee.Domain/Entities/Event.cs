
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("Event")]
public class Event : BaseAuditableEntity
{
	public long Id { get; set; }
	public string? Title { get; set; }
	public string? Image { get; set; }
	public string? Description { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public string StartTime { get; set; }
	public string EndTime { get; set; }
	public string? Location { get; set; }

	public long PetCoffeeShopId { get; set; }
	public PetCoffeeShop PetCoffeeShop { get; set; }

	[InverseProperty(nameof(SubmittingEvent.Event))]
	public IList<SubmittingEvent> SubmittingEvents { get; set; }
	[InverseProperty(nameof(EventField.Event))]
	public IList<EventField> EventFields { get; set; }
	[InverseProperty(nameof(JoinEvent.Event))]
	public IList<JoinEvent> FollowEvents { get; set; }
}
