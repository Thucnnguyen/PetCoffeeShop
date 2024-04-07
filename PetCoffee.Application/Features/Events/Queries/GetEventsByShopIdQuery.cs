using LinqKit;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Domain.Entities;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace PetCoffee.Application.Features.Events.Queries;

public class GetEventsByShopIdQuery : PaginationRequest<Event>, IRequest<PaginationResponse<Event, EventForCardResponse>>
{
	[JsonIgnore]
	[BindNever]
	public long ShopId { get; set; }

	public override Expression<Func<Event, bool>> GetExpressions()
	{

		Expression = Expression.And(e => e.PetCoffeeShopId == ShopId && !e.Deleted);

		return Expression;
	}
}
