
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Packages.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

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
		var package = await _unitOfWork.PackagePromotionRespository.GetByIdAsync(request.PackageId);
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
			shop.EndTimePackage.Value.AddMonths(package.Duration);
		}
		else
		{
			shop.EndTimePackage = DateTimeOffset.UtcNow.AddMonths(package.Duration);
		}

		await _unitOfWork.PetCoffeeShopRepository.UpdateAsync(shop);
		await _unitOfWork.WalletRepsitory.UpdateAsync(wallet);

		await _unitOfWork.SaveChangesAsync();

		return true;
	}
}
