using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service.Notifications;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using Quartz;

namespace PetCoffee.Infrastructure.Scheduler;


public class CheckEventHasEnoughParticipantsJob : IJob
{
	public const string DeleteAccountNotVerifyJobKey = "CheckEventHasEnoughParticipantsJob";

	public const string EventIdKey = "EventId";
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<DeleteAccountNotVerifyJob> _logger;
	private readonly INotifier _notifier;

	public CheckEventHasEnoughParticipantsJob(IUnitOfWork unitOfWork, ILogger<DeleteAccountNotVerifyJob> logger, INotifier notifier)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
		_notifier = notifier;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		var dataMap = context.JobDetail.JobDataMap;
		var eventId = dataMap.GetLongValue(EventIdKey);

		if (eventId == 0)
		{
			return;
		}

		var existedEvent = await _unitOfWork.EventRepository
						.Get(e => e.Id == eventId)
						.Include(e => e.SubmittingEvents)
						.ThenInclude(se => se.CreatedBy)
						.FirstOrDefaultAsync();

		if (existedEvent == null)
		{
			return;
		}

		if (existedEvent.SubmittingEvents.Count() <= existedEvent.MinParticipants)
		{
			//update event
			existedEvent.Status = EventStatus.Closed;

			await _unitOfWork.EventRepository.UpdateAsync(existedEvent);
			await _unitOfWork.SaveChangesAsync();

			//send noti who joined event
			foreach (var submit in existedEvent.SubmittingEvents)
			{
				var notification = new Notification(
					account: submit.CreatedBy,
					type: NotificationType.CancelOrder,
					entityType: EntityType.Event,
					data: existedEvent
				);
				await _notifier.NotifyAsync(notification, true);
			}
		}
		_logger.LogInformation("Check event {eventid} at {time}", existedEvent.Id, DateTime.UtcNow);

	}
}
