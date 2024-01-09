
using LockerService.Domain.Entities;
using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("Post")]
public class Post : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }
	public string? Title { get; set; }
	public string? Content { get; set; }
	public PostStatus Status { get; set; }
	public int CommentCount { get; set; }
	public int LikeCount { get; set; }

	public long PetCafeShopId { get; set; }
	public PetCafeShop PetCafeShop { get; set; }

	public IList<Comment> Comments { get; set; } = new List<Comment>();
}
