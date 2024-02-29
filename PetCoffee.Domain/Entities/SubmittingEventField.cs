

using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Domain.Entities;

public class SubmittingEventField : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }
	public string FieldName { get; set; }
	public string FieldValue { get; set; }
	public bool IsOptional { get; set; }
	public string? OptionValue { get; set; }
	public string? Answer { get; set; }
	public int Order { get; set; }
	public string? Submitcontent { get; set; }

	public long SubmittingEventId { get; set; }
	public SubmittingEvent SubmittingEvent { get; set; }
}
