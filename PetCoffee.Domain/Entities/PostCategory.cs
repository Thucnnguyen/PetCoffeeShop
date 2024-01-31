namespace PetCoffee.Domain.Entities;

public class PostCategory : BaseAuditableEntity
{
	public long PostId { get; set; }
	public long CategoryId { get; set; }

	public Post Post { get; set; }
	public Category Category { get; set; }
}
