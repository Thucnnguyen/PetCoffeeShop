
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
	public PetCoffeeShop PetCoffeeShop { get; set; }
	[InverseProperty(nameof(Comment.Post))]
	public IList<Comment> Comments { get; set; } = new List<Comment>();
	[InverseProperty(nameof(Like.Post))]
	public IList<Like> Likes { get; set; } = new List<Like>();
	[InverseProperty(nameof(PostCategory.Post))]
	public IList<PostCategory> PostCategories { get; set; } = new List<PostCategory>();
	[InverseProperty(nameof(Report.Post))]
	public IList<Report> Reports { get; set; } = new List<Report>();
}
