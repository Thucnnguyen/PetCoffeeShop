

using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Wallets.Models;
using PetCoffee.Application.Features.Wallets.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;
using System.Linq;
using static Google.Apis.Requests.BatchRequest;

namespace PetCoffee.Application.Features.Wallets.Handlers;

public class GetIncomeTransactionForShopByMonthHandler : IRequestHandler<GetIncomeTransactionForShopInMonthQuery, GetTransactionAmountResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public GetIncomeTransactionForShopByMonthHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<GetTransactionAmountResponse> Handle(GetIncomeTransactionForShopInMonthQuery request, CancellationToken cancellationToken)
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
		if (request.From.Month == DateTimeOffset.UtcNow.Month)
		{
			return new GetTransactionAmountResponse();
		}

		var preFrom = request.From.AddMonths(-1);
		var preTo = request.To.AddMonths(-1);
		if (request.ShopId == null)
		{
			var shopIds = currentAccount.AccountShops.Select(acs => acs.ShopId).ToList();

			var shops = await _unitOfWork.PetCoffeeShopRepository
				.Get(s => shopIds.Contains(s.Id) && !s.Deleted && s.Status == ShopStatus.Active)
				.ToListAsync();

			var transactionTypes = new[]
						{
							TransactionType.Donate,
							TransactionType.Reserve,
							TransactionType.AddProducts
						};
			//get all current transaction for all shops
			var curTransactions = await _unitOfWork.TransactionRepository
								.Get(tr => tr.PetCoffeeShopId != null && shopIds.Contains(tr.PetCoffeeShopId.Value)
								&& (tr.CreatedAt <= request.To && tr.CreatedAt >= request.From) 
								&& (tr.TransactionType == TransactionType.Donate ||
									tr.TransactionType == TransactionType.Reserve ||
									tr.TransactionType == TransactionType.AddProducts))
								.ToListAsync();

			var preTransactions = await _unitOfWork.TransactionRepository
								.Get(tr => tr.PetCoffeeShopId != null && shopIds.Contains(tr.PetCoffeeShopId.Value)
								&& (tr.CreatedAt <= preFrom && tr.CreatedAt >= preTo)
								&& (tr.TransactionType == TransactionType.Donate ||
									tr.TransactionType == TransactionType.Reserve ||
									tr.TransactionType == TransactionType.AddProducts))
								.ToListAsync();

			var TransactionDic = new Dictionary<TransactionType, decimal>();
			decimal PreTotal = 0;
			decimal CurTotal = 0;
			//get inconme each months
			foreach (var shop in shops)
			{

				//get all transaction in month
				var curTransaction = curTransactions.Where(tr => tr.PetCoffeeShopId == shop.Id);
				if (!curTransaction.Any())
				{
					continue;
				}

				CurTotal += curTransaction.Sum(tr => tr.Amount);

				var preTransaction = preTransactions.Where(tr => tr.PetCoffeeShopId == shop.Id);
				PreTotal += preTransaction.Sum(tr => tr.Amount);

				foreach (var type in transactionTypes)
				{
					if (TransactionDic.ContainsKey(type))
					{
						TransactionDic[type] += curTransaction.Where(tr => tr.TransactionType == type).Sum(tr => tr.Amount);
					}
					else
					{
						TransactionDic.Add(type, curTransaction.Where(tr => tr.TransactionType == type).Sum(tr => tr.Amount));
					}
				}
			}
			var response = new GetTransactionAmountResponse()
			{
				Balance = CurTotal,
				Percent = PreTotal == 0 ? 0 : (decimal)(CurTotal / PreTotal) * 100,
				IsUp = CurTotal > PreTotal,
				Transactions = curTransactions.Any() ? TransactionDic.Select(td => new TransactionAmountResponse()
				{
					Amount = td.Value,
					TransactionTypes = td.Key,
				}).ToList() : null
			};
			return response;
		}


		var shopById = await _unitOfWork.PetCoffeeShopRepository
			.Get(s => s.Id == request.ShopId && !s.Deleted)
			.FirstOrDefaultAsync();
		if (shopById == null)
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}


		//get all transaction in month
		var curTransactionForShop = await _unitOfWork.TransactionRepository
								.Get(tr => tr.PetCoffeeShopId == request.ShopId
								&& (tr.CreatedAt <= request.To && tr.CreatedAt >= request.From) 
								&& (tr.TransactionType == TransactionType.Donate ||
									tr.TransactionType == TransactionType.Reserve ||
									tr.TransactionType == TransactionType.AddProducts))
								.ToListAsync();

		var TotalIncomeForShop = curTransactionForShop.Sum(tr => tr.Amount);

		var preTransactionForShop = await _unitOfWork.TransactionRepository
								.Get(tr => tr.PetCoffeeShopId == request.ShopId
								&& (tr.CreatedAt <= preFrom && tr.CreatedAt >= preTo)
								&& (tr.TransactionType == TransactionType.Donate ||
									tr.TransactionType == TransactionType.Reserve ||
									tr.TransactionType == TransactionType.AddProducts))
								.ToListAsync();

		var preTotalIncomeForShop = preTransactionForShop.Sum(tr => tr.Amount);

		var newIncomeForShopResponse = new GetTransactionAmountResponse()
		{
			Balance = TotalIncomeForShop,
			Percent = preTotalIncomeForShop == 0 ? 0 : (decimal)(TotalIncomeForShop / preTotalIncomeForShop) * 100,
			IsUp = TotalIncomeForShop > preTotalIncomeForShop,
		};

		
		if (curTransactionForShop.Any())
		{
			var transactionTypeResponse = new[]
						{
							TransactionType.Donate,
							TransactionType.Reserve,
							TransactionType.AddProducts
						};
			foreach (var type in transactionTypeResponse)
			{
				newIncomeForShopResponse.Transactions.Add(new()
				{
					Amount = curTransactionForShop.Where(tr => tr.TransactionType == type).Sum(tr => tr.Amount),
					TransactionTypes = type
				});
			}
		}
		return newIncomeForShopResponse;
	}
}
