using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("Comment")]
public class Comment: BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }
	public string Content { get; set; }
	public string? Image {  get; set; } 

	public long PostId { get; set; }
	public Post Post { get; set; }

	public long CommentorID { get; set; }
	public Account Commentor { get; set; }

	[ForeignKey("ParentComment")]
	public long? ParentCommentId { get; set; }
	public Comment? ParentComment { get; set; }
}
