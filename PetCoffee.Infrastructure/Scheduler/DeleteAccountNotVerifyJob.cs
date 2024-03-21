using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetCoffee.Application.Persistence.Repository;
using Quartz;

namespace PetCoffee.Infrastructure.Scheduler;

public class DeleteAccountNotVerifyJob : IJob
{
    public const string DeleteAccountNotVerifyJobKey = "DeleteAccountNotVerifyJobKey";

    public const string AccountIdKey = "AccountId";
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteAccountNotVerifyJob> _logger;

    public DeleteAccountNotVerifyJob(IUnitOfWork unitOfWork, ILogger<DeleteAccountNotVerifyJob> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;

        var AccountId = dataMap.GetLongValue(AccountIdKey);
        if (AccountId == 0)
        {
            return;
        }

        var Account = await _unitOfWork.AccountRepository.Get(s => s.Id == AccountId && s.IsVerify).FirstOrDefaultAsync();
        if (Account == null)
        {
            return;
        }

        await _unitOfWork.AccountRepository.DeleteAsync(Account);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Invalidate expired Account {AccountName} not verify at {time}", Account.FullName, DateTime.Now);

    }
}
