

namespace PetCoffee.Application.Features.Payments.Models;

public class TransactionItemResponse
{
	public string ItemName { get; set; } 
	public decimal? Price { get; set; }
	public string? Icon { get; set; }
	public int TotalItem { get; set; }
}
