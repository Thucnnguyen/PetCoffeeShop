namespace PetCoffee.Application.Service;

public interface ISchedulerService
{
	Task DeleteAccountNotVerify(long accountId, DateTimeOffset time);
	Task CancelTransactionJob (long transactionId, DateTimeOffset time);
	Task SetReservationToOvertime(long ReservationId, DateTimeOffset time);
	Task CheckEventHasEnoughParticipantJob (long EventId, DateTimeOffset time);
	Task NotiforChangePetArea (string areaIds, DateTimeOffset time);
}
