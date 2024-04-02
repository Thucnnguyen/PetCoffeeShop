
namespace PetCoffee.Domain.Entities;

public class WalletItem
{
	public long ItemId { get; set; }
	public long WalletId { get; set; }
	public int TotalItem { get; set; }
	public decimal PriceItem { get; set; }

	public Item Item { get; set; }
	public Wallet Wallet { get; set; }
}
