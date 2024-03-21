using EntityFrameworkCore.Projectables;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;

public class BaseAuditableEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [ForeignKey("CreatedBy")]
    public long? CreatedById { get; set; }
    public Account? CreatedBy { get; set; }

    public DateTimeOffset UpdatedAt { get; set; } = DateTime.Now;

    public DateTimeOffset? DeletedAt { get; set; }

    [Projectable]
    public bool Deleted => DeletedAt != null;

}