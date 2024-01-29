using EntityFrameworkCore.Projectables;
using PetCoffee.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace LockerService.Domain.Entities;

public class BaseAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    
    public long CreatedById { get; set; }
    public Account CreatedBy { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public long? UpdatedBy { get; set; }
    
    public string? UpdatedByUsername { get; set; }
    
    public DateTime? DeletedAt { get; set; }
    
    public long? DeletedBy { get; set; }
    
    public string? DeletedByUsername { get; set; }

    [Projectable]
    public bool Deleted => DeletedAt != null;
    
}