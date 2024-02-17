using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Post.Models;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Post.Model;

public class PostResponse : BaseAuditableEntityResponse
{
	public long Id { get; set; }
	public string? Title { get; set; }
	public string? Content { get; set; }
	public PostStatus Status { get; set; }
	public string? Image {  get; set; } 
	public AccountForPostModel Account {  get; set; }
	public IList<CommentForPost> Comments { get; set; } = new List<CommentForPost>();
	public IList<CategoryForPostModel> Categories { get; set; } = new List<CategoryForPostModel>();
	public IList<CoffeeshopForPostModel> PetCoffeeShops { get; set; } = new List<CoffeeshopForPostModel>();

}
