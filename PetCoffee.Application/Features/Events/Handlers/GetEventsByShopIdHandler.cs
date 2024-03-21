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

public class GetEventsByShopIdHandler : IRequestHandler<GetEventsByShopIdQuery, PaginationResponse<Event, EventForCardResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentAccountService _currentAccountService;
    private readonly IMapper _mapper;

    public GetEventsByShopIdHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentAccountService = currentAccountService;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<Event, EventForCardResponse>> Handle(GetEventsByShopIdQuery request, CancellationToken cancellationToken)
    {
        var currentAccount = await _currentAccountService.GetCurrentAccount();
        if (currentAccount == null)
        {
            throw new ApiException(ResponseCode.AccountNotExist);
        }
        if (currentAccount.IsVerify)
        {
            throw new ApiException(ResponseCode.AccountNotActived);
        }


        var events = await _unitOfWork.EventRepository.Get(e => e.PetCoffeeShopId == request.ShopId && !e.Deleted)
                                                            .Include(e => e.SubmittingEvents)
                                                            .OrderByDescending(e => e.CreatedAt)
                                                            .ToListAsync();
        var eventResponses = events
               .Skip((request.PageNumber - 1) * request.PageSize)
               .Take(request.PageSize)
               .ToList();
        var response = new List<EventForCardResponse>();
        foreach (var e in eventResponses)
        {
            var eventResponse = _mapper.Map<EventForCardResponse>(e);

            eventResponse.IsJoin = e.SubmittingEvents.Any(e => e.CreatedById == currentAccount.Id);
            response.Add(eventResponse);
        }

        return new PaginationResponse<Event, EventForCardResponse>(
                response,
                events.Count(),
                request.PageNumber,
                request.PageSize);
    }
}
