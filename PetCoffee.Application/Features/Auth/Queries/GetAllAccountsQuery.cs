
using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Auth.Queries;

public class GetAllAccountsQuery : PaginationRequest<Account>, IRequest<PaginationResponse<Account, AccountForRecord>>
{
	private string? _search;

	public string? Search
	{
		get => _search;
		set => _search = value?.Trim().ToLower();
	}
	public AccountStatus? Status { get; set; }
	public Role? Roles { get; set; }
	public override Expression<Func<Account, bool>> GetExpressions()
	{
		if(Search is not null)
		{
			Expression = Expression.And(a => a.Email.ToLower().Contains(Search.ToLower()) || a.PhoneNumber == Search);
		}
		
		if(Status is not null)
		{
			Expression = Expression.And(a => a.Status == Status);
		}
		if (Roles is not null)
		{
			Expression = Expression.And(a => a.Role == Roles);
		}
		Expression = Expression.And(a => a.Role != Role.Admin);
		return Expression;
	}
}
