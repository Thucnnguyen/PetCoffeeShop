
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;

public class PackagePromotion
{
	[Key]
	public long Id { get; set; }
	public string? Description { get; set; }
	public int Duration { get; set; } // month
	public decimal PromotionAmount { get; set; }
	public decimal? PromotionDiscount { get; set; }

	[InverseProperty(nameof(Transaction.PackagePromotion))]
	public IList<Transaction> Transactions { get; set; } = new List<Transaction>();
}
