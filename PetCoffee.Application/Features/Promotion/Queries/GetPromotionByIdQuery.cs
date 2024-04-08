using MediatR;
using PetCoffee.Application.Features.Promotion.Models;


namespace PetCoffee.Application.Features.Promotion.Queries
{
	public class GetPromotionByIdQuery : IRequest<PromotionResponse>
	{
		public long Id { get; set; }
	}

}
