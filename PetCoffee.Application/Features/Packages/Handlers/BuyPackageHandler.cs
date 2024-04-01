
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Packages.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Packages.Handlers;

public class BuyPackagehandler : IRequestHandler<BuyPackageCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public BuyPackagehandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<bool> Handle(BuyPackageCommand request, CancellationToken cancellationToken)
	{
		//check permission
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
		// check package
		var package = await _unitOfWork.PackagePromotionRespository.Get(pp => pp.Id == request.PackageId && !pp.Deleted).FirstOrDefaultAsync();
		if (package == null)
		{
			throw new ApiException(ResponseCode.PackageNotExist);
		}
		//check shop
		var shop = await _unitOfWork.PetCoffeeShopRepository
				.Get(s => s.Id == request.ShopId && !s.Deleted)
				.FirstOrDefaultAsync();
		if (shop == null)
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}

		var wallet = await _unitOfWork.WalletRepsitory
					.Get(w => w.CreatedById == currentAccount.Id)
					.FirstOrDefaultAsync();
		if (wallet == null)
		{
			throw new ApiException(ResponseCode.NotEnoughBalance);
		}

		var totalPrice = package.PromotionAmount - (package.PromotionAmount * package.PromotionDiscount) / 100;

		if(wallet.Balance < totalPrice)
		{
			throw new ApiException(ResponseCode.NotEnoughBalance);
		}

		wallet.Balance -= totalPrice.Value;
		if(shop.EndTimePackage.HasValue)
		{
			shop.EndTimePackage = shop.EndTimePackage.Value.AddMonths(package.Duration);
		}
		else
		{
			shop.EndTimePackage = shop.EndTimePackage = DateTimeOffset.UtcNow.AddMonths(package.Duration);
		}

		await _unitOfWork.PetCoffeeShopRepository.UpdateAsync(shop);
		await _unitOfWork.WalletRepsitory.UpdateAsync(wallet);

		var adminAccount = await _unitOfWork.AccountRepository.Get(a => a.IsAdmin).FirstOrDefaultAsync();
		var adminWallet = await _unitOfWork.WalletRepsitory.Get(w => w.CreatedById == adminAccount.Id).FirstOrDefaultAsync();
		if (adminWallet == null)
		{
			var newWallet = new Wallet((decimal)totalPrice.Value);
			await _unitOfWork.WalletRepsitory.AddAsync(newWallet);
			await _unitOfWork.SaveChangesAsync();
			newWallet.CreatedById = adminAccount.Id;
			await _unitOfWork.WalletRepsitory.UpdateAsync(newWallet);
			await _unitOfWork.SaveChangesAsync();
			adminWallet = newWallet;
		}
		else
		{
			adminWallet.Balance += (decimal)totalPrice.Value;
			await _unitOfWork.WalletRepsitory.UpdateAsync(adminWallet);
		}
		var packageTransaction = new Transaction()
		{
			Amount = (decimal)totalPrice.Value,
			PackagePromotionId = package.Id,
			PetCoffeeShopId = shop.Id,
			Content = "Mua gói gia hạn cho của hàng",
			RemitterId = adminWallet.Id,
			TransactionType = TransactionType.Package,
			TransactionStatus = TransactionStatus.Done,
			ReferenceTransactionId = TokenUltils.GenerateOTPCode(6),
			WalletId = wallet.Id,
		};
		await _unitOfWork.TransactionRepository.AddAsync(packageTransaction);
		await _unitOfWork.SaveChangesAsync();

		return true;
	}
}
