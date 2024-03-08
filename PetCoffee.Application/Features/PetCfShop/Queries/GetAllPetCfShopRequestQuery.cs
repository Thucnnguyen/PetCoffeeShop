using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.PetCfShop.Queries;

public class GetAllPetCfShopRequestQuery : PaginationRequest<PetCoffeeShop>, IRequest<PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>>
{
    private string? _search;

    public string? Search
    {
        get => _search;
        set => _search = value?.Trim().ToLower();
    }

    public ShopType? ShopType { get; set; }

    public double Longitude { get; set; }
    public double Latitude { get; set; }



	public override Expression<Func<PetCoffeeShop, bool>> GetExpressions()
    {
        if (Search is not null)
        {
            Expression = Expression.And(shop => shop.Name != null && shop.Name.ToLower().Contains(Search));
        }

        if (ShopType is not null)
        {
            Expression = Expression.And(shop => Equals(ShopType, shop.Type));
        }

        Expression = Expression.And(shop => shop.Status != ShopStatus.Processing);


        return Expression;
    }
}
