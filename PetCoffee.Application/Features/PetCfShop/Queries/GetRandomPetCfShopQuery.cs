
using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.PetCfShop.Queries;

public class GetRandomPetCfShopQuery : IRequest<IList<PetCoffeeShopForCardResponse>>
{
	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public int Size { get; set; }
	public ShopType? ShopType { get; set; }
}
