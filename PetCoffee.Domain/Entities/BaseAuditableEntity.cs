using EntityFrameworkCore.Projectables;

namespace PetCoffee.Domain.Entities;

public class BaseAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public long? CreatedById { get; set; }

	public Account? CreatedBy { get; set; }

	public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
    


    [Projectable]
    public bool Deleted => DeletedAt != null;
    
}