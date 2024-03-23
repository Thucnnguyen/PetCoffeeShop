using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities
{
	[Table("Product")]
	public class Product : BaseAuditableEntity
	{

		[Key]
		public long Id { get; set; }
		public decimal Price { get; set; }
		public string Name { get; set; }

		public ProductStatus ProductStatus { get; set; } = ProductStatus.Active;

		public string Image { get; set; }



		[InverseProperty(nameof(ReservationProduct.Product))]
		public IList<ReservationProduct> Invoices { get; set; } = new List<ReservationProduct>();



		public long PetCoffeeShopId { get; set; }
		public PetCoffeeShop PetCoffeeShop { get; set; }




	}
}
