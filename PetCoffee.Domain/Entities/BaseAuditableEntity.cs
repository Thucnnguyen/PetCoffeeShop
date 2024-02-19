using EntityFrameworkCore.Projectables;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;

public class BaseAuditableEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;
	[ForeignKey("CreatedBy")]
	public long? CreatedById { get; set; }
	public Account? CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

	public DateTime? DeletedAt { get; set; }

    [Projectable]
    public bool Deleted => DeletedAt != null;
    
}