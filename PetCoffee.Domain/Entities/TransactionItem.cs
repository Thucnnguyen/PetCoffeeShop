

namespace PetCoffee.Domain.Entities;

public class TransactionItem
{
	public long ItemId { get; set; }
	public long TransactionId { get; set; }
	public int TotalItem {  get; set; }

	public Item Item { get; set; }
	public Transaction Transaction { get; set; }
}
