using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Enums;
using Quartz;

namespace PetCoffee.Infrastructure.Scheduler;

public class CancelTransactionJob : IJob
{
	public const string CancelTransactionJobJobKey = "CancleTransactionJob";

	public const string TransactionIdKey = "TransactionId";
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<DeleteAccountNotVerifyJob> _logger;

	public CancelTransactionJob(IUnitOfWork unitOfWork, ILogger<DeleteAccountNotVerifyJob> logger)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		var dataMap = context.JobDetail.JobDataMap;
		var TransactionId = dataMap.GetLongValue(TransactionIdKey);

		if (TransactionId == 0)
		{
			return;
		}

		var transaction = await _unitOfWork.TransactionRepository.Get(s => s.Id == TransactionId).FirstOrDefaultAsync();
		if (transaction == null)
		{
			return;
		}
		if (transaction.TransactionStatus == TransactionStatus.Processing)
		{
			transaction.TransactionStatus = TransactionStatus.Cancel;
		}

		await _unitOfWork.TransactionRepository.UpdateAsync(transaction);
		await _unitOfWork.SaveChangesAsync();

		_logger.LogInformation("Invalidate expired Account {transactionId} not verify at {time}", transaction.Id, DateTime.UtcNow);

	}
}
