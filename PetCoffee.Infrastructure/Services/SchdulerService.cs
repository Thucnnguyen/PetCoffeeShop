

using Microsoft.Extensions.Logging;
using PetCoffee.Application.Service;
using PetCoffee.Infrastructure.Scheduler;
using Quartz;

namespace PetCoffee.Infrastructure.Services;

public class SchdulerService : ISchedulerService
{
	private readonly ILogger<SchdulerService> _logger;

	private readonly ISchedulerFactory _schedulerFactory;

	public SchdulerService(ILogger<SchdulerService> logger, ISchedulerFactory schedulerFactory)
	{
		_logger = logger;
		_schedulerFactory = schedulerFactory;
	}

	public async Task DeleteAccountNotVerify(long accountId, DateTime time)
	{
		try
		{
			_logger.LogInformation("Account with ID: {fullName} will be delete at {time} if not verify", accountId, time);

			var scheduler = await _schedulerFactory.GetScheduler();
			await scheduler.Start();

			var job = JobBuilder.Create<DeleteAccountNotVerifyJob>()
					.UsingJobData(DeleteAccountNotVerifyJob.AccountIdKey, accountId)
					.Build();

			var trigger = TriggerBuilder.Create()
					.StartAt(time)
					.Build();
			await scheduler.ScheduleJob(job, trigger);
		}catch(Exception ex)
		{
			_logger.LogError("Schedule to clear expired account error {error}", ex.Message);
		}
	}
}
