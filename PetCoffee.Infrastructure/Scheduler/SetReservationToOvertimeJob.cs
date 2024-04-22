

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Enums;
using Quartz;

namespace PetCoffee.Infrastructure.Scheduler;

public class SetReservationToOvertimeJob : IJob
{
	public const string SetReservationToOvertimeJobKey = "SetReservationToOvertimeJob";

	public const string ReservationIdKey = "ReservationId";
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<SetReservationToOvertimeJob> _logger;

	public SetReservationToOvertimeJob(IUnitOfWork unitOfWork, ILogger<SetReservationToOvertimeJob> logger)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		var dataMap = context.JobDetail.JobDataMap;
		var ReservationId = dataMap.GetLongValue(ReservationIdKey);

		if (ReservationId == 0)
		{
			return;
		}

		var reservation = await _unitOfWork.ReservationRepository
								.Get(r => r.Id == ReservationId && r.Status == OrderStatus.Success)
								.FirstOrDefaultAsync();
		if (reservation == null)
		{
			return;
		}
		
		reservation.Status = OrderStatus.Overtime;

		await _unitOfWork.ReservationRepository.UpdateAsync(reservation);
		await _unitOfWork.SaveChangesAsync();
	}
}
