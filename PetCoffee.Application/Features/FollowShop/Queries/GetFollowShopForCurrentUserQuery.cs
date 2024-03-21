

using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.FollowShop.Queries;

public class GetFollowShopForCurrentUserQuery : PaginationRequest<FollowPetCfShop>, IRequest<PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>>
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public ShopType? ShopType { get; set; }
    public string? Search { get; set; }
    public override Expression<Func<FollowPetCfShop, bool>> GetExpressions()
    {
        if (ShopType is not null)
        {
            Expression = Expression.And(follow => Equals(ShopType, follow.Shop.Type));
        }
        if (Search is not null)
        {
            Expression = Expression.And(follow => follow.Shop.Name.ToLower().Contains(Search.ToLower().Trim()));
        }
        return Expression;
    }
}
