

using MediatR;

namespace PetCoffee.Application.Features.FollowShop.Commands;

public class DeleteFollowShopCommand : IRequest<bool>
{
    public long PetCoffeeShopId { get; set; }
}
