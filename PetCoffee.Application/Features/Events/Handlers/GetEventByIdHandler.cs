

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

public class GetEventByIdHandler : IRequestHandler<GetEventByIdQuery, EventResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public GetEventByIdHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<EventResponse> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
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

		if (currentAccount.PetCoffeeShopId == null)
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		}

		var getEvent = await _unitOfWork.EventRepository.Get(e => e.Id == request.EventId)
															.Include(e => e.EventFields)
															.FirstOrDefaultAsync();
		if (getEvent == null)
		{
			throw new ApiException(ResponseCode.EventNotExisted);
		}
		
		var response = _mapper.Map<EventResponse>(getEvent);
		response.Fields = getEvent.EventFields.Select(e => _mapper.Map<FieldEventResponseForEventResponse>(e)).ToList();
		return response;
	}
}
