using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Payments.Models;
using PetCoffee.Domain.Enums;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Payments.Queries;

public class GetAllTransactionByCurrentWalletQuery : PaginationRequest<Domain.Entities.Transaction>, IRequest<PaginationResponse<Domain.Entities.Transaction, PaymentResponse>>
{
	public DateTimeOffset? From { get; set; }

	public DateTimeOffset? To { get; set; }

	public TransactionStatus? Status { get; set; }

	public TransactionType? Type { get; set; }

	private string? _search;

	public string? Search
	{
		get => _search;
		set => _search = value?.Trim().ToLower();
	}
	public override Expression<Func<Domain.Entities.Transaction, bool>> GetExpressions()
	{

		if (From != null)
		{
			Expression = Expression.And(Transaction => Transaction.CreatedAt >= From);
		}

		if (To != null)
		{
			Expression = Expression.And(Transaction => Transaction.CreatedAt <= To);
		}

		if (Status != null)
		{
			Expression = Expression.And(Transaction => Transaction.TransactionStatus == Status);
		}

		if (Search != null)
		{
			Expression = Expression
				.And(transaction => transaction.CreatedBy.PhoneNumber == Search || transaction.ReferenceTransactionId == Search || transaction.CreatedBy.FullName.ToLower().Contains(Search.ToLower()));
		}

		return Expression;
	}
}
