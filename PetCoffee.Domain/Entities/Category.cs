using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;

public class Category : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }
	public string Name { get; set; }
	[InverseProperty(nameof(PostCategory.Category))]
	public IList<PostCategory> PostCategories { get; set; } = new List<PostCategory>();
}
