using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Product.Models;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Product.Queries
{


	public class GetProductsByShopIdQuery : PaginationRequest<Domain.Entities.Product>, IRequest<PaginationResponse<Domain.Entities.Product, ProductResponse>>
	{
		public long ShopId { get; set; }

        private string? _search;
        public string? Search
        {
            get => _search;
            set => _search = value?.Trim().ToLower();
        }

        public override Expression<Func<Domain.Entities.Product, bool>> GetExpressions()
		{
            if (Search is not null)
            {
                Expression = Expression.And(p => (p.Name != null && p.Name.ToLower().Contains(Search)));

            }

            Expression = Expression.And(p => p.PetCoffeeShopId == ShopId && !p.Deleted);

            return Expression;

        }
    }
}
