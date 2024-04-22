
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service.Notifications;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using Quartz;

namespace PetCoffee.Infrastructure.Scheduler;

public class CheckShopHasPetJob : IJob
{
	public const string CheckShopHasPetJobKey = "CheckShopHasPetJob";

	private readonly IUnitOfWork _unitOfWork;
	private readonly INotifier _notifier;
	private readonly ILogger<CheckShopHasPetJob> _logger;

	public CheckShopHasPetJob(IUnitOfWork unitOfWork, ILogger<CheckShopHasPetJob> logger, INotifier notifier)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
		_notifier = notifier;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		var shopsNotHavePet = await _unitOfWork.PetCoffeeShopRepository
									.Get(ps => !ps.Pets.Any() && !ps.Deleted && ps.Status == ShopStatus.Active)
									.ToListAsync();
		if (!shopsNotHavePet.Any())
		{
			return;
		}

		var shopIds = shopsNotHavePet.Select(s => s.Id);
		var managerAccounts = await _unitOfWork.AccountRepository
									.Get(a => a.IsManager && a.AccountShops.Any(sId => shopIds.Any(id => id == sId.ShopId)) && !a.Deleted)
									.Include(a => a.AccountShops)
									.ToListAsync();

		foreach (var shop in shopsNotHavePet)
		{
			var manager = managerAccounts.FirstOrDefault(acc => acc.AccountShops.Any(acs => acs.ShopId == shop.Id));
			if (manager == null)
			{
				continue;
			}
			var notificationForPoster = new Notification(
					account: manager,
					type: NotificationType.RemindShopAboutNotHavingPet,
					entityType: EntityType.Shop,
					data: shop,
					shopId: shop.Id
				);
			await _notifier.NotifyAsync(notificationForPoster, true);
		}
	}
}
