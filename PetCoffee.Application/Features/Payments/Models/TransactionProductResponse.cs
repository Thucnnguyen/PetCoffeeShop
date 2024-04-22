

namespace PetCoffee.Application.Features.Payments.Models;

public class TransactionProductResponse
{
	public long Quantity { get; set; }
	public decimal Price { get; set; }
	public string? ProductImage { get; set; }
	public string ProductName { get; set; }
}
