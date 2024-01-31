using EntityFrameworkCore.Projectables;
using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("Notification")]
public class Notification : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }

	public string Title { get; set; }
	public string Content { get; set; }
	public string Data { get; set; }
	public EntityType EntityType { get; set; }
	public string? ReferenceId { get; set; }
	public DateTimeOffset? ReadAt { get; set; }
	public NotificationLevel Level { get; set; } = NotificationLevel.Information;

	public long AccountId { get; set; }
	public Account Account { get; set; }
	[Projectable]
	public bool IsRead => ReadAt != null || Deleted;
}
