
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Application.Features.Events.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.Events.Handlers;

public class GetAllSubmittingEventByEventIdForShopHandler : IRequestHandler<GetAllSubmittingEventByEventIdForShopQuery, EventResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public GetAllSubmittingEventByEventIdForShopHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<EventResponse> Handle(GetAllSubmittingEventByEventIdForShopQuery request, CancellationToken cancellationToken)
	{
		var submittingEvents = await _unitOfWork.SubmittingEventRepsitory
				.Get(se => se.EventId == request.EventId)
				.Include(se => se.Event)
				.ThenInclude(e => e.FollowEvents)
				.Include(se => se.CreatedBy)
				.Include(s => s.SubmittingEventFields)
				.ToListAsync();
		if(!submittingEvents.Any()) 
		{
			return new EventResponse();
		}

		var answer = new Dictionary<string, SubmittingEventField>();
		foreach(var se in submittingEvents)
		{
			foreach( var sef in se.SubmittingEventFields)
			{
				if (answer.ContainsKey(sef.Question))
				{
					answer[sef.Question].SubmittingContent += ";" + sef.SubmittingContent;
				}
				else
				{
					answer.Add(sef.Question, sef);
				}
			}
		}

		var answerResponse = answer.Values.Select(a => _mapper.Map<FieldEventResponseForEventResponse>(a)).ToList();

		var response = _mapper.Map<EventResponse>(submittingEvents.FirstOrDefault());
		response.Fields = answerResponse;

		return response;
	}
}
