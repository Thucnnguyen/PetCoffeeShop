
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Domain.Entities;

public class PackagePromotion
{
	[Key]
	public long Id { get; set; }
	public string? Description { get; set; } 
	public int Duration { get; set; } // month
	public decimal PromotionAmount { get; set; }
	public decimal? PromotionDiscount { get; set; }
}
