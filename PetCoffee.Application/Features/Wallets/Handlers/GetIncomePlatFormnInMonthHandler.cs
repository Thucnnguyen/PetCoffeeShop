
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Features.Wallets.Models;
using PetCoffee.Application.Features.Wallets.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Wallets.Handlers;

public class GetIncomePlatFormnInMonthHandler : IRequestHandler<GetIncomePlatFormnInMonthQuery, GetTransactionAmountResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public GetIncomePlatFormnInMonthHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<GetTransactionAmountResponse> Handle(GetIncomePlatFormnInMonthQuery request, CancellationToken cancellationToken)
	{
		if (request.From.Month == DateTimeOffset.UtcNow.Month)
		{
			return new GetTransactionAmountResponse();
		}

		var preFrom = request.From.AddMonths(-1);
		var preTo = request.To.AddMonths(-1);

		var curTransaction = await _unitOfWork.TransactionRepository
								.Get(tr => tr.TransactionType == TransactionType.Package
								&& (tr.CreatedAt <= request.To && tr.CreatedAt >= request.From))
								.ToListAsync();

		if (curTransaction == null)
		{
			return new GetTransactionAmountResponse();
		}

		var TotalIncomeShop = curTransaction.Sum(ct => ct.Amount);
		


		var preTransaction = await _unitOfWork.TransactionRepository
						.Get(tr => tr.TransactionType == TransactionType.Package
						&& (tr.CreatedAt <= preFrom && tr.CreatedAt >= preTo))
						.ToListAsync();

		var preTotalIncomeShop = curTransaction.Sum(ct => ct.Amount);

		var response = new GetTransactionAmountResponse()
		{
			Balance = TotalIncomeShop,
			Percent = preTotalIncomeShop == 0 ? 0 : (decimal)(TotalIncomeShop / preTotalIncomeShop) * 100,
			IsUp = TotalIncomeShop > preTotalIncomeShop,
			Transactions = curTransaction.Any() ? new()
			{
				new()
				{
					Amount = TotalIncomeShop,
					TransactionTypes = TransactionType.Package,
				}
			} : null
		};
		return response;
	}
}
