﻿
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Wallets.Models;
using PetCoffee.Application.Features.Wallets.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Wallets.Handlers;

public class GetShopIncomeHandler : IRequestHandler<GetShopIncomeQuery, IncomeResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public GetShopIncomeHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<IncomeResponse> Handle(GetShopIncomeQuery request, CancellationToken cancellationToken)
	{
		var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
		if (currentAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (currentAccount.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}

		if (!currentAccount.AccountShops.Any(acs => acs.ShopId == request.ShopId))
		{
			throw new ApiException(ResponseCode.PermissionDenied);

		}

		var shop = await _unitOfWork.PetCoffeeShopRepository
			.Get(s => s.Id == request.ShopId && !s.Deleted)
			.FirstOrDefaultAsync();

		if (shop == null)
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}
		var monthAmounts = new LinkedList<decimal>();
		for (var i = request.Months - 1; i >= 0; i--)
		{

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
							.Get(tr => tr.PetCoffeeShopId == shop.Id
							&& (tr.CreatedAt <= lastDayOfMonth && tr.CreatedAt >= firstDayOfMonth))
							.ToListAsync();


			var TotalIncomeShop =
				transaction.Where(tr => tr.TransactionType == TransactionType.Donate).Sum(tr => tr.Amount) +
				transaction.Where(tr => tr.TransactionType == TransactionType.Reserve).Sum(tr => tr.Amount) +
				transaction.Where(tr => tr.TransactionType == TransactionType.AddProducts).Sum(tr => tr.Amount);

			monthAmounts.AddLast(TotalIncomeShop);
		}

		var IncomeResponse = new IncomeResponse()
		{
			Balanace = monthAmounts.Sum(),
			MonthAmounts = monthAmounts.ToList()
		};
		return IncomeResponse;
	}
}
