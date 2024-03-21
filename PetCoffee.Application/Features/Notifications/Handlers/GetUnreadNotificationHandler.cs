using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Notifications.Models;
using PetCoffee.Application.Features.Notifications.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Notifications.Handlers;

public class GetUnreadNotificationHandler : IRequestHandler<GetUnreadNotificationQuery, UnreadNotificationCountResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentAccountService _currentAccountService;

    public GetUnreadNotificationHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
    {
        _unitOfWork = unitOfWork;
        _currentAccountService = currentAccountService;
    }

    public async Task<UnreadNotificationCountResponse> Handle(GetUnreadNotificationQuery request, CancellationToken cancellationToken)
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

        var count = await _unitOfWork.NotificationRepository
            .Get(notification => !notification.IsRead
                                 && notification.AccountId == currentAccount.Id
                                 && (request.Type == null || Equals(notification.Type, request.Type))
                                 && (request.EntityType == null || Equals(notification.EntityType, request.EntityType))
                                 && (request.From == null || Equals(notification.CreatedAt >= request.From))
                                 && (request.To == null || Equals(notification.CreatedAt <= request.To)))
            .CountAsync(cancellationToken);

        return new UnreadNotificationCountResponse(count);
    }
}
