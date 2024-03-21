
using MediatR;

namespace PetCoffee.Application.Features.FollowShop.Commands;

public class CreateFollowShopCommand : IRequest<bool>
{
    public long PetCoffeeShopId { get; set; }
}
