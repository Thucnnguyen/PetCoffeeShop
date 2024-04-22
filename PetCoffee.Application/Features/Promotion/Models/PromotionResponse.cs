using PetCoffee.Application.Common.Models.Response;
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Application.Features.Promotion.Models
{
	public class PromotionResponse : BaseAuditableEntityResponse
	{
		[Key]
		public long Id { get; set; }
		public string Code { get; set; }
		public DateTimeOffset From { get; set; }
		public DateTimeOffset To { get; set; }

		public int Quantity { get; set; }
		public int Percent { get; set; }

		public long PetCoffeeShopId { get; set; }
		public bool IsUsed { get; set; }
		public int Available { get; set; } = 0;

	}
}
