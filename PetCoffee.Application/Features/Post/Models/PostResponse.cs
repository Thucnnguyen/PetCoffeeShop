using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Post.Model;

public class PostResponse : BaseAuditableEntityResponse
{
    public long Id { get; set; }
    public string NamePoster { get; set; }
    public string PosterAvatar { get; set; }
    public string? Content { get; set; }
    public PostStatus Status { get; set; }
    public string? Image { get; set; }
    public long? ShopId { get; set; }
    public long AccountId { get; set; }
    public IList<CategoryForPostModel> Categories { get; set; } = new List<CategoryForPostModel>();
    public IList<CoffeeshopForPostModel> PetCoffeeShops { get; set; } = new List<CoffeeshopForPostModel>();


    public double? TotalLike { get; set; } = 0;
    public double? TotalComment { get; set; } = 0;
    public bool IsLiked { get; set; } = false;
}
