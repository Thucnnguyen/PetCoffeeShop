using MediatR;
using PetCoffee.Application.Features.Product.Models;
using PetCoffee.Application.Features.Promotion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Promotion.Queries
{
	public class GetPromotionByIdQuery : IRequest<PromotionResponse>
	{
		public long Id { get; set; }
	}

}
