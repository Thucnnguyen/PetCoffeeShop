using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.PetCfShop.Queries;

public class GetAllPetCfShopQuery : PaginationRequest<PetCoffeeShop>, IRequest<PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>>
{
    private string? _search;

    public string? Search
    {
        get => _search;
        set => _search = value?.Trim().ToLower();
    }

    public ShopType? ShopType { get; set; }

    public string? Address { get; set; }
	public double Latitude { get; set; }
	public double Longitude { get; set; }

	public override Expression<Func<PetCoffeeShop, bool>> GetExpressions()
    {
        if (Search is not null)
        {
            Expression = Expression.And(store => store.Name != null && store.Name.ToLower().Contains(Search));
        }

        if (ShopType is not null)
        {
            Expression = Expression.And(store => Equals(ShopType, store.Type));
        }

        if (Address is not null)
        {
            Expression = Expression.And(store => store.Location != null && store.Location.ToLower().Contains(Address.ToLower().Trim()));
        }

        return Expression;
    }
}
