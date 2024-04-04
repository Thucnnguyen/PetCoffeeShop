
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Features.Wallets.Models;
using PetCoffee.Application.Features.Wallets.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Wallets.Handlers;

public class GetIncomePlatformHandler : IRequestHandler<GetIncomePlatformQuery, IncomeResponse>
{
	private readonly IUnitOfWork _unitOfWork;

	public GetIncomePlatformHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<IncomeResponse> Handle(GetIncomePlatformQuery request, CancellationToken cancellationToken)
	{
		//list for store amount
		var monthAmounts = new LinkedList<decimal>();
		for (var i = request.Months - 1; i >= 0; i--)
		{
			// get month
			var month = DateTimeOffset.UtcNow.AddMonths(-i);
			var firstDayOfMonth = new DateTimeOffset(month.Year, month.Month, 1, 0, 0, 0, month.Offset);
			var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
			if (i == 0)
			{
				monthAmounts.AddLast(0);
				continue;
			}
			//get all transaction in month
			var transaction = await _unitOfWork.TransactionRepository
							.Get(tr => tr.TransactionType == TransactionType.Package
							&& (tr.CreatedAt <= lastDayOfMonth && tr.CreatedAt >= firstDayOfMonth))
							.ToListAsync();
			monthAmounts.AddLast(transaction.Sum(s => s.Amount));
		}
		var IncomeResponse = new IncomeResponse()
		{
			Balanace = monthAmounts.Sum(),
			MonthAmounts = monthAmounts.ToList()
		};
		return IncomeResponse;
	}
}
