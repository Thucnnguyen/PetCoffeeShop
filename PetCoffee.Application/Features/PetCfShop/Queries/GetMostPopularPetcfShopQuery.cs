

using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.PetCfShop.Queries;

public class GetMostPopularPetcfShopQuery : PaginationRequest<PetCoffeeShop>, IRequest<PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>>
{

	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public ShopType? ShopType { get; set; }
	public override Expression<Func<PetCoffeeShop, bool>> GetExpressions()
	{
		if (ShopType is not null)
		{
			Expression = Expression.And(store => Equals(ShopType, store.Type));
		}
		Expression = Expression.And(Shop => Shop.Status == ShopStatus.Active && Shop.IsBuyPackage);
		return Expression;
	}
}
