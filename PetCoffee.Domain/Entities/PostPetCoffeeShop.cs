namespace PetCoffee.Domain.Entities;

public class PostPetCoffeeShop : BaseAuditableEntity
{
    public long PostId { get; set; }
    public Post Post { get; set; }

    public long ShopId { get; set; }
    public PetCoffeeShop Shop { get; set; }
}
