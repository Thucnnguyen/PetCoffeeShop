

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Application.Features.Events.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System.Net.WebSockets;

namespace PetCoffee.Application.Features.Events.Handlers;

public class GetEventsByShopIdHandler : IRequestHandler<GetEventsByShopIdQuery, List<EventForCardResponse>>
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

	public async Task<List<EventForCardResponse>> Handle(GetEventsByShopIdQuery request, CancellationToken cancellationToken)
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


		var events = await _unitOfWork.EventRepository.Get(e => e.PetCoffeeShopId == request.ShopId)
															.Include(e => e.SubmittingEvents)
															.ToListAsync();

		var response = events.Select(e =>
		{
			var eventResponse = _mapper.Map<EventForCardResponse>(e);
			eventResponse.TotalJoinEvent = e.SubmittingEvents.Count();
			return eventResponse;
		}).ToList();

		return response;
	}
}
