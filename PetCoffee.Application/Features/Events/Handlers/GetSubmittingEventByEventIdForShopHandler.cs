using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Application.Features.Events.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.Events.Handlers;

public class GetSubmittingEventByEventIdForShopHandler : IRequestHandler<GetSubmittingEventByEventIdForShopQuery, PaginationResponse<SubmittingEvent, EventSubmittingForCardResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentAccountService _currentAccountService;
    private readonly IMapper _mapper;

    public GetSubmittingEventByEventIdForShopHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentAccountService = currentAccountService;
        _mapper = mapper;
    }

    async Task<PaginationResponse<SubmittingEvent, EventSubmittingForCardResponse>> IRequestHandler<GetSubmittingEventByEventIdForShopQuery, PaginationResponse<SubmittingEvent, EventSubmittingForCardResponse>>.Handle(GetSubmittingEventByEventIdForShopQuery request, CancellationToken cancellationToken)
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

        var SubmitingEvents = _unitOfWork.SubmittingEventRepsitory
                            .Get(predicate: request.GetExpressions())
                            .Include(se => se.Event)
                            .ThenInclude(e => e.FollowEvents)
                            .Include(se => se.CreatedBy)
                            .OrderByDescending(se => se.CreatedAt)
                            .AsQueryable();


        return new PaginationResponse<SubmittingEvent, EventSubmittingForCardResponse>(
                SubmitingEvents,
                request.PageNumber,
                request.PageSize,
                se => _mapper.Map<EventSubmittingForCardResponse>(se));
    }
}
