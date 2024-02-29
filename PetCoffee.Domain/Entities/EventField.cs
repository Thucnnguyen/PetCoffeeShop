

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;

public class EventField
{
	[Key]
	public long Id { get; set; }
	public string FieldName { get; set; }
	public string FieldValue { get; set; }
	public bool IsOptional { get; set; }
	public string? OptionValue { get; set; }
	public string? Answer {  get; set; }
	public int Order { get; set; }

	public long EventId { get; set; }
	public Event Event { get; set; }

}
