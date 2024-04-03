
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Transactions.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Transactions.Handlers;

public class BuyItemHandler : IRequestHandler<BuyItemsCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public BuyItemHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<bool> Handle(BuyItemsCommand request, CancellationToken cancellationToken)
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

		var items = new List<Item>();
		var itemDic = new Dictionary<long, int>();
		foreach (var item in request.Items)
		{
			var existedItem = await _unitOfWork.ItemRepository.Get(i => i.ItemId == item.ItemId).FirstOrDefaultAsync();	
			if (existedItem == null)
			{
				throw new ApiException(ResponseCode.ItemNotExist);
			}

			if (existedItem.Deleted)
			{
				throw new ApiException(ResponseCode.CannotBuyItem);
			}

			items.Add(existedItem);
			itemDic.Add(item.ItemId, item.Quantity);
		}

		var wallet = await _unitOfWork.WalletRepsitory.Get(w => w.CreatedById == currentAccount.Id)
					.FirstOrDefaultAsync();
		if (wallet == null)
		{
			throw new ApiException(ResponseCode.NotEnoughBalance);
		}

		double totalMoney = 0;
		foreach (var item in items)
		{
			totalMoney += item.Price * itemDic[item.ItemId];
		}

		if ((decimal)totalMoney > wallet.Balance)
		{
			throw new ApiException(ResponseCode.NotEnoughBalance);
		}

		wallet.Balance -= (decimal)totalMoney;
		foreach (var item in items)
		{
			var itemWallet = await _unitOfWork.WalletItemRepository
								.Get(iw => iw.WalletId == wallet.Id && iw.ItemId == item.ItemId)
								.FirstOrDefaultAsync();
			if (itemWallet == null)
			{
				await _unitOfWork.WalletItemRepository.AddAsync(new WalletItem()
				{
					ItemId = item.ItemId,
					WalletId = wallet.Id,
					TotalItem = itemDic[item.ItemId]
				});
				await _unitOfWork.SaveChangesAsync();
				continue;
			}

			itemWallet.TotalItem += itemDic[item.ItemId];
			await _unitOfWork.WalletItemRepository.UpdateAsync(itemWallet);
			await _unitOfWork.SaveChangesAsync();

		}
		//var adminAccount = await _unitOfWork.AccountRepository.Get(a => a.IsAdmin).FirstOrDefaultAsync();
		//var adminWallet = await _unitOfWork.WalletRepsitory.Get(w => w.CreatedById == adminAccount.Id).FirstOrDefaultAsync();
		//if (adminWallet == null)
		//{
		//	var newWallet = new Wallet((decimal)totalMoney);
		//	await _unitOfWork.WalletRepsitory.AddAsync(newWallet);
		//	await _unitOfWork.SaveChangesAsync();
		//	newWallet.CreatedById = adminAccount.Id;
		//	await _unitOfWork.WalletRepsitory.UpdateAsync(newWallet);
		//	adminWallet = newWallet;
		//}
		//else
		//{
		//	adminWallet.Balance += (decimal)totalMoney;
		//	await _unitOfWork.WalletRepsitory.UpdateAsync(adminWallet);
		//}
		var donateTransaction = new Transaction()
		{
			Amount = (decimal)totalMoney,
			Content = "Mua quà Tặng",
			//RemitterId = adminWallet.Id,
			TransactionType = TransactionType.BuyItem,
			TransactionStatus = TransactionStatus.Done,
			ReferenceTransactionId = TokenUltils.GenerateOTPCode(6),
			WalletId = wallet.Id,
		};
		await _unitOfWork.TransactionRepository.AddAsync(donateTransaction);
		await _unitOfWork.SaveChangesAsync();
		var Itemtransactions = new List<TransactionItem>();
		foreach (var item in items)
		{
			Itemtransactions.Add(new TransactionItem
			{
				ItemId = item.ItemId,
				TotalItem = itemDic[item.ItemId],
				TransactionId = donateTransaction.Id,
			});

		}
		await _unitOfWork.TransactionItemRepository.AddRange(Itemtransactions);
		await _unitOfWork.SaveChangesAsync();

		return true;
	}
}
