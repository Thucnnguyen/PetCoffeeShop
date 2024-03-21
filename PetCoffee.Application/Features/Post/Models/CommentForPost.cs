namespace PetCoffee.Application.Features.Post.Models;

public class CommentForPost
{
	public long Id { get; set; }
	public string? Content { get; set; }
	public string? Image { get; set; }
	public long? ParentId { get; set; }
	public long CommentorID { get; set; }
	public string CommentorName { get; set; }
}
