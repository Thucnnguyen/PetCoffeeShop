
namespace PetCoffee.Domain.Entities;

public class JoinEvent : BaseAuditableEntity
{
    public long EventId { get; set; }

    public Event Event { get; set; }
}
