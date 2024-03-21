using EntityFrameworkCore.Projectables;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;

public class BaseAuditableEntity
{
	public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
	[ForeignKey("CreatedBy")]
	public long? CreatedById { get; set; }
	public Account? CreatedBy { get; set; }

	public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

	public DateTimeOffset? DeletedAt { get; set; }

	[Projectable]
	public bool Deleted => DeletedAt != null;

}