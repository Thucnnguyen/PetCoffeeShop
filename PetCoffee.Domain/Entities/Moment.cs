using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Domain.Entities;

public class Moment : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }
	public string? Content { get; set; }
	public string? Image { get; set; }
	public MomentType MomentType { get; set; }

	public long PetId { get; set; }
	public Pet Pet { get; set; }
}
