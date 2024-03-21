

namespace PetCoffee.Domain.Entities;

public class RatePet : BaseAuditableEntity
{
    public long PetId { get; set; }
    public Pet Pet { get; set; }
    public long Rate { get; set; }
    public long Comment { get; set; }
}
