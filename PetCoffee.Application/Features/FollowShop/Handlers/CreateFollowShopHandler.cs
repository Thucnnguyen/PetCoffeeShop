

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.FollowShop.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Application.Service.Notifications;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.FollowShop.Handlers;

internal class CreateFollowShopHandler : IRequestHandler<CreateFollowShopCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly INotifier _notifier;

	public CreateFollowShopHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService, INotifier notifier)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_currentAccountService = currentAccountService;
		_notifier = notifier;
	}

	public async Task<bool> Handle(CreateFollowShopCommand request, CancellationToken cancellationToken)
	{
		var curAccount = await _currentAccountService.GetCurrentAccount();
		if (curAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (curAccount.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}

		var shop = await _unitOfWork.PetCoffeeShopRepository
			.Get(s => s.Id == request.PetCoffeeShopId && !s.Deleted)
			.FirstOrDefaultAsync();

		if (shop == null)
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}

		var NewFollowShop = new FollowPetCfShop() { ShopId = request.PetCoffeeShopId };

		await _unitOfWork.FollowPetCfShopRepository.AddAsync(NewFollowShop);
		await _unitOfWork.SaveChangesAsync();
		var newFollowShopData = await _unitOfWork.FollowPetCfShopRepository
								.Get(f => f.ShopId == NewFollowShop.ShopId && f.CreatedById == NewFollowShop.CreatedById)
								.Include(p => p.Shop)
								.Include(p => p.CreatedBy)
								.FirstOrDefaultAsync();
		var managerAccount = await _unitOfWork.AccountRepository
			.Get(a => a.IsManager && a.AccountShops.Any(ac => ac.ShopId == newFollowShopData.ShopId))
			.FirstOrDefaultAsync();
		if (managerAccount != null) 
		{
			var notification = new Notification(
					account: managerAccount,
					type: NotificationType.NewFollower,
					entityType: EntityType.Shop,
					data: newFollowShopData
				);
			await _notifier.NotifyAsync(notification, true);
		}
		
		return true;
	}
}
