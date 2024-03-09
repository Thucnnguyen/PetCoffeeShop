using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Domain.Entities;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Auth.Queries;

public class GetStaffAccountByShopIdQuery : PaginationRequest<Account>, IRequest<PaginationResponse<Account, AccountResponse>>
{
	public long ShopId { get; set; }
	private string? _search;

	public string? Search
	{
		get => _search;
		set => _search = value?.Trim().ToLower();
	}
	public override Expression<Func<Account, bool>> GetExpressions()
	{
		return Expression;

	}
}
