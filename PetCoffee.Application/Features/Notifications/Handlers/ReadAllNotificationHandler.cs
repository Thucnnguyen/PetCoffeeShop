
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Notifications.Commands;
using PetCoffee.Application.Features.Notifications.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Notifications.Handlers;

public class ReadAllNotificationHandler : IRequestHandler<ReadAllNotificationCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public ReadAllNotificationHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<bool> Handle(ReadAllNotificationCommand request, CancellationToken cancellationToken)
	{
		var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
		if (currentAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (currentAccount.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}


		var notifications = await _unitOfWork.NotificationRepository.GetAsync(n => n.AccountId == currentAccount.Id && n.IsRead);

		if (notifications == null)
		{
			return true;
		}

		foreach(var noti in notifications)
		{
			noti.ReadAt = DateTimeOffset.UtcNow;
			await _unitOfWork.NotificationRepository.UpdateAsync(noti);
		}

		await _unitOfWork.SaveChangesAsync();
		return true;
	}
}
