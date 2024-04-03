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

public class GetIncomeHandler : IRequestHandler<GetIncomeQuery, IncomeForShopResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;


	public GetIncomeHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<IncomeForShopResponse> Handle(GetIncomeQuery request, CancellationToken cancellationToken)
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

		var shopIds = currentAccount.AccountShops.Select(acs => acs.ShopId).ToList();

		var shops = await _unitOfWork.PetCoffeeShopRepository
			.Get(s => shopIds.Contains(s.Id) && !s.Deleted)
			.ToListAsync();

		if (!shops.Any())
		{
			return new();
		}
		var shopDic = new Dictionary<string, LinkedList<decimal>>();
		//get inconme each months
		for (var i = 0; i < request.Months; i++)
		{
			var month = DateTimeOffset.UtcNow.AddMonths(-i);
			var firstDayOfMonth = new DateTimeOffset(month.Year, month.Month, 1, 0, 0, 0, month.Offset);
			var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
			foreach (var shop in shops)
			{

				//get all transaction in month
				var transaction = await _unitOfWork.TransactionRepository
								.Get(tr => tr.PetCoffeeShopId == shop.Id
								&& (tr.CreatedAt <= lastDayOfMonth && tr.CreatedAt >= firstDayOfMonth))
								.ToListAsync();

				//var TotalIncomeShop =
				//	transaction.Where(tr => tr.TransactionType == TransactionType.Donate).Sum(tr => tr.Amount) +
				//	transaction.Where(tr => tr.TransactionType == TransactionType.Reserve).Sum(tr => tr.Amount) +
				//	transaction.Where(tr => tr.TransactionType == TransactionType.AddProducts).Sum(tr => tr.Amount) -
				//	transaction.Where(tr => tr.TransactionType == TransactionType.Refund).Sum(tr => tr.Amount) -
				//	transaction.Where(tr => tr.TransactionType == TransactionType.RemoveProducts).Sum(tr => tr.Amount);

				var TotalIncomeShop =
					transaction.Where(tr => tr.TransactionType == TransactionType.Donate).Sum(tr => tr.Amount) +
					transaction.Where(tr => tr.TransactionType == TransactionType.Reserve).Sum(tr => tr.Amount) +
					transaction.Where(tr => tr.TransactionType == TransactionType.AddProducts).Sum(tr => tr.Amount);

				if (!shopDic.ContainsKey(shop.Name))
				{
					shopDic.Add(shop.Name, new LinkedList<decimal>());
					shopDic[shop.Name].AddLast(TotalIncomeShop);
				}
				else
				{
					shopDic[shop.Name].AddLast(TotalIncomeShop);
				}
			}
		}
		var IncomeResponse = new IncomeForShopResponse()
		{
			Balanace = shopDic.Sum(s => s.Value.Sum()),
			MonthAmounts = shopDic
			.Select(sd => new ShopIncomeResponse()
			{
				ShopName = sd.Key,
				Incomes = sd.Value,
			}).ToList()
		};

		return IncomeResponse;
	}
}
