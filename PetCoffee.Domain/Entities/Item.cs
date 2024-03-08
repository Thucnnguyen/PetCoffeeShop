
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;

public class Item : BaseAuditableEntity
{
	[Key]
	public long ItemId {  get; set; }
	public string Name { get; set; }	
	public double Price { get; set; }
	public string? Description { get; set; }
	public string? Icon {  get; set; } 

	[InverseProperty(nameof(TransactionItem.Item))]
	public IList<TransactionItem> Transactions { get; set; } = new List<TransactionItem>();

}
