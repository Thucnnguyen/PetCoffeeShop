

using PetCoffee.Application.Common.Models.Response;

namespace PetCoffee.Application.Features.Comment.Models;

public class CommentResponse : BaseAuditableEntityResponse
{
	public long Id { get; set; }
	public string CommentorName { get; set; }
	public string CommentorImage { get; set; }
	public string Content { get; set; }
	public string? Image { get; set; }
	public long PostId { get; set; }
	public long? ParentCommentId { get; set; }
	public long? ShopId { get; set; }
	public long TotalSubComments { get; set; } = 0;
}
