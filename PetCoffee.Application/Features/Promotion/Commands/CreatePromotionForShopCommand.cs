using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.PetCfShop.Commands;
using PetCoffee.Application.Features.Product.Models;
using PetCoffee.Application.Features.Promotion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Promotion.Commands
{
	public class CreatePromotionForShopValidation : AbstractValidator<CreatePromotionForShopCommand>
	{
		public CreatePromotionForShopValidation()
		{
			RuleFor(model => model.From).NotEmpty();
			RuleFor(model => model.To).NotEmpty();
			RuleFor(model => model.Quantity).NotEmpty().GreaterThan(0);

			RuleFor(model => model.Percent)
				.GreaterThan(0).LessThanOrEqualTo(100);
		}
	}

	public class CreatePromotionForShopCommand : IRequest<PromotionResponse>
	{

		public long PetCoffeeShopId { get; set; }
		//public string Code { get; set; }
		public DateTimeOffset From { get; set; }
		public DateTimeOffset To { get; set; }

		public int Quantity { get; set; }
		public int Percent { get; set; }

		
	}
}
