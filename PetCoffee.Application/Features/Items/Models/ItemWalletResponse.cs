
namespace PetCoffee.Application.Features.Items.Models;

public class ItemWalletResponse
{
	public long ItemId { get; set; }
	public string Name { get; set; }
	public string? Icon { get; set; }
	public double Price { get; set; }
	public int TotalItem {  get; set; }
}
