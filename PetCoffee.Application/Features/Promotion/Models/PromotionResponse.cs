using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Promotion.Models
{
	public class PromotionResponse: BaseAuditableEntityResponse
	{
		[Key]
		public long Id { get; set; }
		public string Code { get; set; }
		public DateTimeOffset From { get; set; }
		public DateTimeOffset To { get; set; }

		public int Quantity { get; set; }
		public int Percent { get; set; }

		public long PetCoffeeShopId { get; set; }
		//public PetCoffeeShop PetCoffeeShop { get; set; }

		public int Avaliable { get; set; }

	}
}
