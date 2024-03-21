namespace PetCoffee.Application.Service;

public interface ISchedulerService
{
    Task DeleteAccountNotVerify(long accountId, DateTime time);

}
