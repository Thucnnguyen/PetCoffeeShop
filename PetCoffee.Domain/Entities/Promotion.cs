
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PetCoffee.Domain.Entities
{
	[Table("Promotion")]
	public class Promotion : BaseAuditableEntity
	{
		[Key]
		public long Id { get; set; }
		public string Code { get; set; }
		public DateTimeOffset From { get; set; }
		public DateTimeOffset To { get; set; }

		public int Quantity { get; set; }
		public int Percent { get; set; }

		public long PetCoffeeShopId { get; set; }
		public PetCoffeeShop PetCoffeeShop { get; set; }


		[InverseProperty(nameof(AccountPromotion.Promotion))]
		public IList<AccountPromotion> AccountPromotions { get; set; } = new List<AccountPromotion>();


	}
}
