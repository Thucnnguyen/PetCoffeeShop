
namespace PetCoffee.Domain.Entities;

public class FollowPetCfShop : BaseAuditableEntity
{
	public long ShopId { get; set; }
	public PetCoffeeShop Shop { get; set; }
}
