using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Domain.Entities;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Events.Queries;

public class GetEventsForCustomerQuery : PaginationRequest<Event>, IRequest<PaginationResponse<Event, EventResponse>>
{
	private string? _search;

	public string? Search
	{
		get => _search;
		set => _search = value?.Trim().ToLower();
	}

	public override Expression<Func<Event, bool>> GetExpressions()
	{
		if (Search is not null)
		{
			Expression = Expression.And(e => e.Title.ToLower().Contains(Search) || e.Description.ToLower().Contains(Search));
		}
		return Expression;
	}
}
