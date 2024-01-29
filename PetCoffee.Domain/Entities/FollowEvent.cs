
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Domain.Entities;

public class FollowEvent
{
	[Key]
	public long EventId { get; set; }
	[Key]
	public long AccountId { get; set; }

	public Event Event { get; set; }
	public Account Account { get; set; }
}
