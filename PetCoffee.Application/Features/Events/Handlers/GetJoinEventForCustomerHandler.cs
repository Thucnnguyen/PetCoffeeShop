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

public class GetJoinEventForCustomerHandler : IRequestHandler<GetJoinEventForCustomerQuery, PaginationResponse<SubmittingEvent, EventForCardResponse>>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public GetJoinEventForCustomerHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<PaginationResponse<SubmittingEvent, EventForCardResponse>> Handle(GetJoinEventForCustomerQuery request, CancellationToken cancellationToken)
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
		var Expression = request.GetExpressions().And(se => se.CreatedById == currentAccount.Id);
		var SubmitingEvents = _unitOfWork.SubmittingEventRepsitory
							.Get(predicate: Expression)
							.Include(se => se.Event)
							.ThenInclude(e => e.FollowEvents)
							.OrderByDescending(se => se.CreatedAt)
							.AsQueryable();

		return new PaginationResponse<SubmittingEvent, EventForCardResponse>(
				SubmitingEvents,
				request.PageNumber,
				request.PageSize,
				se => _mapper.Map<EventForCardResponse>(se));
	}
}
