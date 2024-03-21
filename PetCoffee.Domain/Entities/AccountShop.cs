
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;

public class AccountShop
{
    public long AccountId { get; set; }
    [ForeignKey("PetCoffeeShop")]
    public long ShopId { get; set; }

    public Account Account { get; set; }
    public PetCoffeeShop PetCoffeeShop { get; set; }
}
