
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Memory.Models;

public class MomentResponse : BaseAuditableEntity
{
	public long Id { get; set; }
	public string? Content { get; set; }
	public string? Image { get; set; }
	public MomentType MomentType { get; set; }

	public long PetId { get; set; }
}
