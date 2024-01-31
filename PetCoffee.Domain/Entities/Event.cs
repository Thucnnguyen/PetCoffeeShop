
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("Event")]
public class Event : BaseAuditableEntity
{
	public long Id { get; set; }
	public string? Title { get; set; }
	public string? Content { get; set; }
	public string? Image {  get; set; }
	public string? Description { get; set; }	
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
	public string? Location { get; set; }

	public long PetCafeShopId { get; set; }
	public PetCoffeeShop PetCoffeeShop { get; set; }

	[InverseProperty(nameof(SubmittingEvent.Event))]
	public IList<SubmittingEvent> SubmittingEvents { get; set;}
	[InverseProperty(nameof(EventField.Event))]
	public IList<EventField> EventFields { get; set; }
	[InverseProperty(nameof(FollowEvent.Event))]
	public IList<FollowEvent> FollowEvents { get; set; }
}
