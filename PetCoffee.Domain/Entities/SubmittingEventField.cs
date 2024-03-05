

using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Domain.Entities;

public class SubmittingEventField : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }
	public string Question { get; set; }
	public string Type { get; set; }
	public bool IsOptional { get; set; }
	public string? Answer { get; set; }
	public string? Submitcontent { get; set; }

	public long SubmittingEventId { get; set; }
	public SubmittingEvent SubmittingEvent { get; set; }
}
