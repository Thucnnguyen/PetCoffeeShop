

using LinqKit;
using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Payments.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Payments.Queries;

public class GetAllTransactionQuery : PaginationRequest<Domain.Entities.Transaction>, IRequest<PaginationResponse<Domain.Entities.Transaction, PaymentResponse>>
{
	public DateTimeOffset? From { get; set; }

	public DateTimeOffset? To { get; set; }

	public TransactionStatus? Status { get; set; }

	public TransactionType? Type { get; set; }
	public long? CustomerId { get; set; }
	public long? ShopId { get; set; }

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

		if (ShopId != null)
		{
			Expression = Expression
				.And(Transaction => (Transaction.Pet != null && Transaction.Pet.PetCoffeeShopId == ShopId)
									|| (Transaction.Reservation != null && Transaction.Reservation.Area != null && Transaction.Reservation.Area.PetcoffeeShopId == ShopId));
		}

		if (CustomerId != null)
		{
			Expression = Expression.And(Transaction => Transaction.CreatedById == CustomerId || Transaction.RemitterId == CustomerId);
		}
		return Expression;
	}
}
