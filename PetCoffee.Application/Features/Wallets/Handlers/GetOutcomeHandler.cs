

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

public class GetOutcomeHandler : IRequestHandler<GetOutcomeQuery, IncomeForShopResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public GetOutcomeHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<IncomeForShopResponse> Handle(GetOutcomeQuery request, CancellationToken cancellationToken)
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
			.Get(s => shopIds.Contains(s.Id) && !s.Deleted && s.Status == ShopStatus.Active)
			.ToListAsync();


		if (!shops.Any())
		{
			return new();
		}
		var shopDic = new Dictionary<string, LinkedList<decimal>>();

		//get All Transaction of shop in n months
		var curDate = DateTimeOffset.UtcNow;
		var getFromDate = new DateTimeOffset(curDate.AddMonths(-(request.Months - 1)).Year,
											 curDate.AddMonths(-(request.Months - 1)).Month, 1, 0, 0, 0, DateTimeOffset.UtcNow.Offset);
		var getToDate = new DateTimeOffset(curDate.Year,
											 curDate.Month, 1, 0, 0, 0, DateTimeOffset.UtcNow.Offset).AddDays(-1);

		var transaction = await _unitOfWork.TransactionRepository
								.Get(tr => tr.PetCoffeeShopId != null && shopIds.Contains(tr.PetCoffeeShopId.Value)
								&& (tr.CreatedAt <= getToDate && tr.CreatedAt >= getFromDate)
								&& (tr.TransactionType == TransactionType.Refund ||
									tr.TransactionType == TransactionType.RemoveProducts ||
									tr.TransactionType == TransactionType.Package))
								.ToListAsync();

		//get inconme each months
		for (var i = request.Months-1; i >= 0; i--)
		{
			var month = DateTimeOffset.UtcNow.AddMonths(-i);
			var firstDayOfMonth = new DateTimeOffset(month.Year, month.Month, 1, 0, 0, 0, month.Offset);
			var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
			foreach (var shop in shops)
			{
				if (i == 0)
				{
					if (!shopDic.ContainsKey(shop.Name))
					{
						shopDic.Add(shop.Name, new LinkedList<decimal>());
						shopDic[shop.Name].AddLast(0);
					}
					else
					{
						shopDic[shop.Name].AddLast(0);
					}
					continue;
				}

				//get all transaction in month
				var shopTransactions = transaction.Where(tr => tr.PetCoffeeShopId == shop.Id
															&& (tr.CreatedAt <= lastDayOfMonth && tr.CreatedAt >= firstDayOfMonth));


				if (!shopDic.ContainsKey(shop.Name))
				{
					shopDic.Add(shop.Name, new LinkedList<decimal>());
					shopDic[shop.Name].AddLast(shopTransactions.Sum(tr => tr.Amount));
				}
				else
				{
					shopDic[shop.Name].AddLast(shopTransactions.Sum(tr => tr.Amount));
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
				Amounts = sd.Value,
			}).ToList()
		};

		return IncomeResponse;
	}
}
