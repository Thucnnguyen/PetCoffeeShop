
using MediatR;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.PetCfShop.Queries;

public class GetRandomPetCfShopQuery : IRequest<IList<PetCoffeeShopForCardResponse>>
{
	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public int Size { get; set; }
	public ShopType? ShopType { get; set; }
}
