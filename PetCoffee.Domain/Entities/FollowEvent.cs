
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;

public class FollowEvent : BaseAuditableEntity
{
	public long EventId { get; set; }

	public Event Event { get; set; }
}
