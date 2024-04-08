using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Promotion.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using TmsApi.Common;

namespace PetCoffee.Application.Features.Promotion.Handlers;

public class UpdatePromotionForShopHandler : IRequestHandler<UpdatePromotionForShopCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public UpdatePromotionForShopHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<bool> Handle(UpdatePromotionForShopCommand request, CancellationToken cancellationToken)
	{
		var currentAccount = await _currentAccountService.GetCurrentAccount();
		if (currentAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (currentAccount.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}

		var promotion = await _unitOfWork.PromotionRepository
							.Get(p => p.Id == request.Id && !p.Deleted)
							.Include(p => p.AccountPromotions)
							.FirstOrDefaultAsync();
		if (promotion == null)
		{
			throw new ApiException(ResponseCode.PromotionNotExisted);
		}

		if (!currentAccount.AccountShops.Any(a => a.ShopId == promotion.PetCoffeeShopId))
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		};


		Assign.Partial<UpdatePromotionForShopCommand, Domain.Entities.Promotion>(request, promotion);

		await _unitOfWork.PromotionRepository.UpdateAsync(promotion);
		await _unitOfWork.SaveChangesAsync();
		return true;
	}
}
