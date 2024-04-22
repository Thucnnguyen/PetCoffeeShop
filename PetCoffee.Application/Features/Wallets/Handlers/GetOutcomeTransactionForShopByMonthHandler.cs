using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Wallets.Models;
using PetCoffee.Application.Features.Wallets.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Wallets.Handlers;

public class GetOutcomeTransactionForShopByMonthHandler : IRequestHandler<GetOutcomeTransactionForShopInMonthQuery, GetTransactionAmountResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public GetOutcomeTransactionForShopByMonthHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<GetTransactionAmountResponse> Handle(GetOutcomeTransactionForShopInMonthQuery request, CancellationToken cancellationToken)
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
				.Get(s => shopIds.Contains(s.Id) && !s.Deleted)
				.ToListAsync();


			var TransactionDic = new Dictionary<TransactionType, List<Transaction>>();
			decimal PreTotal = 0;
			decimal CurTotal = 0;
			var transactionTypes = new[]
						{
							TransactionType.Refund,
							TransactionType.RemoveProducts,
							TransactionType.Package
						};
			var curTransactions = await _unitOfWork.TransactionRepository
								.Get(tr => tr.PetCoffeeShopId != null && shopIds.Contains(tr.PetCoffeeShopId.Value)
								&& (tr.CreatedAt <= request.To && tr.CreatedAt >= request.From)
								&& (tr.TransactionType == TransactionType.Package ||
									tr.TransactionType == TransactionType.RemoveProducts ||
									tr.TransactionType == TransactionType.Refund))
								.ToListAsync();

			var preTransactions = await _unitOfWork.TransactionRepository
								.Get(tr => tr.PetCoffeeShopId != null && shopIds.Contains(tr.PetCoffeeShopId.Value)
								&& (tr.CreatedAt <= preFrom && tr.CreatedAt >= preTo)
								&& (tr.TransactionType == TransactionType.Package ||
									tr.TransactionType == TransactionType.RemoveProducts ||
									tr.TransactionType == TransactionType.Refund))
								.ToListAsync();

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
						TransactionDic[type].AddRange(curTransaction.Where(tr => tr.TransactionType == type));
					}
					else
					{
						TransactionDic.Add(type, new());
						TransactionDic[type].AddRange(curTransaction.Where(tr => tr.TransactionType == type));
					}
				}
			}
			var response = new GetTransactionAmountResponse()
			{
				Balance = CurTotal,
				Percent = PreTotal == 0 ? 0 : (decimal)(CurTotal / PreTotal) * 100,
				IsUp = CurTotal > PreTotal,
				Transactions = curTransactions.Any() ? TransactionDic.Where(td => td.Value.Sum(t => t.Amount) > 0)
								.Select(td => new TransactionAmountResponse()
								{
									Amount = td.Value.Sum(t => t.Amount),
									TotalTransaction = td.Value.Count(),
									TransactionTypes = td.Key
								})
								.ToList()
								: null
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
		var newResponse = new LinkedList<GetTransactionAmountResponse>();

		//get all transaction in month
		var curTransactionForShop = await _unitOfWork.TransactionRepository
								.Get(tr => tr.PetCoffeeShopId == shopById.Id
								&& (tr.CreatedAt <= request.To && tr.CreatedAt >= request.From)
								&& (tr.TransactionType == TransactionType.Package ||
									tr.TransactionType == TransactionType.RemoveProducts ||
									tr.TransactionType == TransactionType.Refund))
								.ToListAsync();

		var TotalIncomeForShop = curTransactionForShop.Sum(tr => tr.Amount);

		var preTransactionForShop = await _unitOfWork.TransactionRepository
								.Get(tr => tr.PetCoffeeShopId == shopById.Id
								&& (tr.CreatedAt >= preFrom && tr.CreatedAt <= preTo)
								&& (tr.TransactionType == TransactionType.Package ||
									tr.TransactionType == TransactionType.RemoveProducts ||
									tr.TransactionType == TransactionType.Refund))
								.ToListAsync();

		var preTotalIncomeForShop = preTransactionForShop.Sum(tr => tr.Amount);

		var newIncomeForShopResponse = new GetTransactionAmountResponse()
		{
			Balance = TotalIncomeForShop,
			Percent = preTotalIncomeForShop == 0 ? 0 : (decimal)(TotalIncomeForShop / preTotalIncomeForShop) * 100,
			IsUp = TotalIncomeForShop > preTotalIncomeForShop,
		};

		var transactionTypeResponse = new[]
						{
							TransactionType.Refund,
							TransactionType.RemoveProducts,
							TransactionType.Package
						};
		if (curTransactionForShop.Any())
		{
			newIncomeForShopResponse.Transactions = new();
			foreach (var type in transactionTypeResponse)
			{
				var TotalAmount = curTransactionForShop.Where(tr => tr.TransactionType == type).Sum(tr => tr.Amount);
				if (TotalAmount != 0)
				{
					newIncomeForShopResponse.Transactions.Add(new()
					{
						Amount = TotalAmount,
						TotalTransaction = curTransactionForShop.Where(tr => tr.TransactionType == type).Count(),
						TransactionTypes = type
					});
				}
					
			}
		}

		return newIncomeForShopResponse;
	}
}
