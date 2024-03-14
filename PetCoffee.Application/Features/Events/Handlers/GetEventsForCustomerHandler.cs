
using AutoMapper;
using LinqKit;
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

public class GetEventsForCustomerHandler : IRequestHandler<GetEventsForCustomerQuery, PaginationResponse<Event, EventResponse>>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public GetEventsForCustomerHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<PaginationResponse<Event, EventResponse>> Handle(GetEventsForCustomerQuery request, CancellationToken cancellationToken)
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

		var followedShopIds = (await _unitOfWork.FollowPetCfShopRepository.GetAsync(f => f.CreatedById == currentAccount.Id)).ToList();

		var Expression = request.GetExpressions().And(e => !e.Deleted);

		var eventQuery = _unitOfWork.EventRepository.Get(
			  predicate: Expression,
			  disableTracking: true
			)
			.Include(e => e.EventFields)
			.Include(e => e.SubmittingEvents)
			.AsQueryable();
		if (followedShopIds.Any())
		{
			eventQuery = eventQuery
			 .OrderByDescending(e => e.CreatedAt)
			 .ThenByDescending(e => followedShopIds.Select(f => f.ShopId).Contains(e.PetCoffeeShopId))
			 .ThenByDescending(e => e.SubmittingEvents.Count());
		}
		else
		{
			eventQuery = eventQuery
			 .OrderByDescending(e => e.CreatedAt)
			 .ThenByDescending(e => e.FollowEvents.Count());
		}


		var eventResponses = eventQuery
			   .Skip((request.PageNumber - 1) * request.PageSize)
			   .Take(request.PageSize)
			   .ToList();

		var response = new List<EventResponse>();
		foreach (var e in eventResponses)
		{
			var eventResponse = _mapper.Map<EventResponse>(e);
			if (e.EventFields.Any())
			{
				eventResponse.Fields = e.EventFields.Select(e => _mapper.Map<FieldEventResponseForEventResponse>(e)).ToList();
			}
			eventResponse.IsJoin = e.SubmittingEvents.Any(e => e.CreatedById == currentAccount.Id);
			response.Add(eventResponse);
		}
		return new PaginationResponse<Event, EventResponse>(
				response,
				eventQuery.Count(),
				request.PageNumber,
				request.PageSize);
	}
}
