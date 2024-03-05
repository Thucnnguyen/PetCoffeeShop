using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Notifications.Commands;
using PetCoffee.Application.Features.Notifications.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System.Linq.Dynamic.Core;

namespace PetCoffee.Application.Features.Notifications.Handlers;

public class UpdateNotificationStatusHandler : IRequestHandler<UpdateNotificationStatusCommand, NotificationResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public UpdateNotificationStatusHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<NotificationResponse> Handle(UpdateNotificationStatusCommand request, CancellationToken cancellationToken)
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

		var notification = (await _unitOfWork.NotificationRepository.GetAsync(n => n.Id == request.Id && n.CreatedById == currentAccount.Id)).FirstOrDefault();

		if (notification == null)
		{
			throw new ApiException(ResponseCode.NotificationErrorNotFound);
		}

		if (notification.IsRead == request.IsRead)
		{
			throw new ApiException(ResponseCode.NotificationErrorInvalidStatus);
		}

		notification.ReadAt = request.IsRead != null && request.IsRead.Value ? DateTimeOffset.UtcNow : null;
		await _unitOfWork.NotificationRepository.UpdateAsync(notification);
		await _unitOfWork.SaveChangesAsync();

		return _mapper.Map<NotificationResponse>(notification);
	}
}
