using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Domain.Entities;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Events.Queries;

public class GetSubmittingEventByEventIdForShopQuery : PaginationRequest<SubmittingEvent>, IRequest<PaginationResponse<SubmittingEvent, EventSubmittingForCardResponse>>
{
	public long EventId { get; set; }
	private string? _search;

	public string? Search
	{
		get => _search;
		set => _search = value?.Trim().ToLower();
	}

	public override Expression<Func<SubmittingEvent, bool>> GetExpressions()
	{
		if (Search is not null)
		{
			Expression = Expression.And(e => e.CreatedBy.FullName.ToLower().Contains(Search));
		}

		Expression = Expression.And(e => e.EventId == EventId);
		return Expression;
	}
}
