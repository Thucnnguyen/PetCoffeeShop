

using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Domain.Entities;

public class SubmittingEventField
{
	[Key]
	public long Id { get; set; }
	public long EventFieldId { get; set; }
	public EventField EventField { get; set; }

	public long SubmittingEventId { get; set; }
	public SubmittingEvent SubmittingEvent { get; set; }
	public string? Submitcontent { get; set; }
}
