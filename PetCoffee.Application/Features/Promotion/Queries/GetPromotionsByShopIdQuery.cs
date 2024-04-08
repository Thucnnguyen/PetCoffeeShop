using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Promotion.Models;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Promotion.Queries
{

	public class GetPromotionsByShopIdQuery : PaginationRequest<Domain.Entities.Promotion>, IRequest<PaginationResponse<Domain.Entities.Promotion, PromotionResponse>>
	{
		public long ShopId { get; set; }

		private string? _search;
		public string? Search
		{
			get => _search;
			set => _search = value?.Trim().ToLower();
		}
		public override Expression<Func<Domain.Entities.Promotion, bool>> GetExpressions()
		{
			if (Search is not null)
			{
				Expression = Expression.And(p => (p.Code != null && p.Code.Equals(Search)));

			}

			Expression = Expression.And(p => p.PetCoffeeShopId == ShopId && !p.Deleted);


			return Expression;
		}
	}
}
