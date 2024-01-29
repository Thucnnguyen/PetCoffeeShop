
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Domain.Entities;

public class Floor
{
	[Key]
	public long Id { get; set; }
	public string? Description { get; set; }
	public string? Image { get; set; }
	public int TotalSeat { get; set; }

    public long PetcoffeeShopId { get; set; }
    public PetCoffeeShop PetCoffeeShop { get; set; }
}
