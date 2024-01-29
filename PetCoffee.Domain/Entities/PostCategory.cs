
using LockerService.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Domain.Entities;

public class PostCategory : BaseAuditableEntity
{
	[Key]
	public long PostId { get; set; }
	public long CategoryId { get; set; }

	public Post Post { get; set; }
	public Category Category { get; set; }
}
