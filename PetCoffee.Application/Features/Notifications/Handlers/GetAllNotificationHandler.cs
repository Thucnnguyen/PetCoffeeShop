
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Notifications.Models;
using PetCoffee.Application.Features.Notifications.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.Notifications.Handlers;

public class GetAllNotificationHandler : IRequestHandler<GetAllNotificationQuery, PaginationResponse<Notification, NotificationResponse>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public GetAllNotificationHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<PaginationResponse<Notification, NotificationResponse>> Handle(GetAllNotificationQuery request, CancellationToken cancellationToken)
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

		request.AccountId = currentAccount.Id;
		request.SortDir = SortDirection.Desc;
		request.SortColumn = "CreatedAt";
		var notificationQuery = await _unitOfWork.NotificationRepository.GetAsync(
		   predicate: request.GetExpressions(),
		   orderBy: request.GetOrder(),
		   disableTracking: true);

		return new PaginationResponse<Notification, NotificationResponse>(
		   notificationQuery,
		   request.PageNumber,
		   request.PageSize,
		   notification => _mapper.Map<NotificationResponse>(notification));
	}
}
