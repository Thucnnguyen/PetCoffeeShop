

using MediatR;

namespace PetCoffee.Application.Features.Promotion.Commands;

public class UpdatePromotionForShopCommand : IRequest<bool>
{
	public long Id {  get; set; } 
	public DateTimeOffset? From { get; set; }
	public DateTimeOffset? To { get; set; }

	public int? Quantity { get; set; }
	public int? Percent { get; set; }
}
