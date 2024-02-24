

using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Post.Models;

namespace PetCoffee.Application.Features.Comment.Models;

public class CommentResponse : BaseAuditableEntityResponse
{
	public long Id { get; set; }
	public string Content { get; set; }
	public string? Image { get; set; }
	public long PostId { get; set; }
	public long? ParentCommentId { get; set; }
	public AccountForPostModel Account { get; set; }
}
