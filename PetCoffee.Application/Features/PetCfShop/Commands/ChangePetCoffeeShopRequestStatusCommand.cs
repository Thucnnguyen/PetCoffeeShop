
using MediatR;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.PetCfShop.Commands;

public class ChangePetCoffeeShopRequestStatusCommand : IRequest<bool>
{
    public long ShopId { get; set; }
    public ShopStatus Status { get; set; }
}
