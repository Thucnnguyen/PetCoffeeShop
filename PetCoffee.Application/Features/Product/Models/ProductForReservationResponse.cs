
namespace PetCoffee.Application.Features.Product.Models;

public class ProductForReservationResponse
{
    public long ProductId { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }

    public int Quantity { get; set; }

}
