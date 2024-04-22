

using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Domain.Entities;

public class TransactionProduct
{
	[Key]
	public long Id { get; set; }
	public long ProductId { get; set; }
	public long TransactionId { get; set; }

	public long Quantity { get; set; }
	public decimal Price { get; set; }
	public string ProductName { get; set; }
	public string? ProductImage { get; set; }

	public Product Product { get; set; }
	public Transaction Transaction { get; set; }
}
