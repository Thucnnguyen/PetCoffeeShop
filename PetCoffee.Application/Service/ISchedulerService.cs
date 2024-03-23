namespace PetCoffee.Application.Service;

public interface ISchedulerService
{
	Task DeleteAccountNotVerify(long accountId, DateTimeOffset time);

}
