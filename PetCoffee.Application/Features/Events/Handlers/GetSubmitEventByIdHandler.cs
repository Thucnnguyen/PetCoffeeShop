
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Application.Features.Events.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Events.Handlers;



public class GetSubmitEventByIdHandler : IRequestHandler<GetSubmitEventByIdQuery, EventResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentAccountService _currentAccountService;
    private readonly IMapper _mapper;

    public GetSubmitEventByIdHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentAccountService = currentAccountService;
        _mapper = mapper;
    }

    public async Task<EventResponse> Handle(GetSubmitEventByIdQuery request, CancellationToken cancellationToken)
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

        var GetSubmittingEvent = await _unitOfWork.SubmittingEventRepsitory.Get(e => e.Id == request.SubmittingEventId)
                                                                .Include(s => s.SubmittingEventFields)
                                                                .Include(s => s.Event)
                                                                .FirstOrDefaultAsync();
        if (GetSubmittingEvent == null)
        {
            throw new ApiException(ResponseCode.EventNotExisted);
        }

        var response = _mapper.Map<EventResponse>(GetSubmittingEvent);
        response.IsJoin = GetSubmittingEvent != null;
        if (response.IsJoin)
        {
            if (GetSubmittingEvent.SubmittingEventFields.Any())
                response.Fields = GetSubmittingEvent.SubmittingEventFields.Select(a => _mapper.Map<FieldEventResponseForEventResponse>(a)).ToList();
        }
        return response;
    }
}
