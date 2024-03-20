using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Payments.Models;
using PetCoffee.Application.Features.Transactions.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Application.Service.Notifications;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Transactions.Handlers;

public class DonationForPetHandler : IRequestHandler<DonationForPetCommand, PaymentResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;
	private readonly ICacheService _cacheService;
	private readonly INotifier _notifier;

	public DonationForPetHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper, ICacheService cacheService, INotifier notifier)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
		_cacheService = cacheService;
		_notifier = notifier;
	}

	public async Task<PaymentResponse> Handle(DonationForPetCommand request, CancellationToken cancellationToken)
	{
		//check current account
		var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
		if (currentAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (currentAccount.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}
		//check pet
		var pet = await _unitOfWork.PetRepository
				.Get(p => p.Id == request.PetId && !p.Deleted)
				.FirstOrDefaultAsync();
		if (pet == null)
		{
			throw new ApiException(ResponseCode.PetNotExisted);
		}

		//check wallet is created
		var wallet = await _unitOfWork.WalletRepsitory
				.Get(w => w.CreatedById == currentAccount.Id)
				.FirstOrDefaultAsync();

		if (wallet == null)
		{
			throw new ApiException(ResponseCode.ItemInWalletNotEnough);
		}

		var donationItemIds = request.DonateItems.Select(di => di.ItemId);
		double totalMoney = 0;
		var newTransactionItem = new List<TransactionItem>();
		foreach (var i in request.DonateItems)
		{
			var item = await _unitOfWork.WalletItemRepository
				.Get(it => donationItemIds.Any(di => di == it.ItemId) && it.WalletId == wallet.Id)
				.Include(it => it.Item)
				.FirstOrDefaultAsync();
			if (item == null || item.TotalItem < i.Quantity)
			{
				throw new ApiException(ResponseCode.ItemInWalletNotEnough);
			}
			item.TotalItem -= i.Quantity;
			await _unitOfWork.WalletItemRepository.UpdateAsync(item);
			totalMoney += i.Quantity * item.Item.Price;
			newTransactionItem.Add(new TransactionItem()
			{
				ItemId = i.ItemId,
				TotalItem = i.Quantity,
			});
		}

		//get manager account 
		var managerAccount = await _unitOfWork.AccountRepository
			.Get(a => a.IsManager && a.AccountShops.Any(ac => ac.ShopId == pet.PetCoffeeShopId))
			.FirstOrDefaultAsync();

		var managaerWallet = await _unitOfWork.WalletRepsitory
			.Get(w => w.CreatedById == managerAccount.Id)
			.FirstOrDefaultAsync();
		var newWallet = new Wallet();
		if (managaerWallet == null)
		{
			newWallet.Balance = (decimal)totalMoney;
			await _unitOfWork.WalletRepsitory.AddAsync(newWallet);
			await _unitOfWork.SaveChangesAsync();
			newWallet.CreatedById = managerAccount.Id;
			await _unitOfWork.WalletRepsitory.UpdateAsync(newWallet);
			await _unitOfWork.SaveChangesAsync();

		}
		else
		{
			managaerWallet.Balance += (decimal)totalMoney;
			await _unitOfWork.WalletRepsitory.UpdateAsync(managaerWallet);
			await _unitOfWork.SaveChangesAsync();
		}

		var newTransaction = new Domain.Entities.Transaction()
		{
			WalletId = wallet.Id,
			Amount = (decimal)totalMoney,
			Content = "Tặng quà cho thú cưng",
			PetId = pet.Id,
			RemitterId = managaerWallet != null ? managaerWallet.Id : newWallet.Id,
			TransactionStatus = TransactionStatus.Done,
			ReferenceTransactionId = Guid.NewGuid().ToString(),
			Items = newTransactionItem,
			TransactionType = TransactionType.Donate,

		};
		await _unitOfWork.TransactionRepository.AddAsync(newTransaction);
		await _unitOfWork.SaveChangesAsync();

		newTransaction.CreatedBy = currentAccount;
		await _cacheService.RemoveAsync(pet.Id.ToString(), cancellationToken);
		var notification = new Notification(
			account: managerAccount,
			type: NotificationType.Donation,
			entityType: EntityType.Transaction,
			data: newTransaction
		);
		await _notifier.NotifyAsync(notification, true);

		return _mapper.Map<PaymentResponse>(newTransaction);
	}
}
