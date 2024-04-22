

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service.Notifications;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using Quartz;

namespace PetCoffee.Infrastructure.Scheduler;

public class SendNotiAndUpdateReservationWhenChangePetAreaJob : IJob
{
	public const string SendNotiAndUpdateReservationWhenChangePetAreaJobKey = "SenNotiAndUpdateReservationWhenChangePetAreaJob";

	public const string AreaIdsKey = "AreaIdskey";
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<SendNotiAndUpdateReservationWhenChangePetAreaJob> _logger;
	private readonly INotifier _notifier;

	public SendNotiAndUpdateReservationWhenChangePetAreaJob(IUnitOfWork unitOfWork, ILogger<SendNotiAndUpdateReservationWhenChangePetAreaJob> logger, INotifier notifier)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
		_notifier = notifier;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		var dataMap = context.JobDetail.JobDataMap;
		var areaIds = dataMap.GetString(AreaIdsKey);

		if (string.IsNullOrEmpty(areaIds))
		{
			return;
		}
		var listAreaIds = areaIds.Split(',').Select(a => long.Parse(a)).Distinct().ToList();
		var areas = await _unitOfWork.AreaRepsitory
						.Get(a => listAreaIds.Any(id => id == a.Id))
						.Include(a => a.Reservations.Where(r => r.StartTime > DateTimeOffset.UtcNow))
							.ThenInclude(r => r.CreatedBy)
						.ToListAsync();
		var listResvation = areas.SelectMany(a => a.Reservations).ToList();

		foreach (var resvation in listResvation)
		{
			resvation.IsTotallyRefund = true;
			await _unitOfWork.ReservationRepository.UpdateAsync(resvation);

			var notification = new Notification(
			account: resvation.CreatedBy,
			type: NotificationType.ChangePetArea,
			entityType: EntityType.Reservation,
			data: resvation
			);
			await _notifier.NotifyAsync(notification, true);
		}
		await _unitOfWork.SaveChangesAsync();

		_logger.LogInformation("Change return 100% reservation at {time}", DateTime.UtcNow);

	}
}
