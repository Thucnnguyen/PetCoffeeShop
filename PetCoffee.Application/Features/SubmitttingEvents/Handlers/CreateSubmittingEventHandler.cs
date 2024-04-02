using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.SubmitttingEvents.Commands;
using PetCoffee.Application.Features.SubmitttingEvents.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.SubmitttingEvents.Handlers;

public class CreateSubmittingEventHandler : IRequestHandler<CreateSubmittingEventCommand, SubmittingEventResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public CreateSubmittingEventHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<SubmittingEventResponse> Handle(CreateSubmittingEventCommand request, CancellationToken cancellationToken)
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

		var CheckEvent = await _unitOfWork.EventRepository
								.Get(e => e.Id == request.EventId)
								.Include(e => e.SubmittingEvents)
								.FirstOrDefaultAsync();

		if (CheckEvent == null)
		{
			throw new ApiException(ResponseCode.EventNotExisted);
		}
		if (CheckEvent.SubmittingEvents.Any(se => se.CreatedById == currentAccount.Id))
		{
			throw new ApiException(ResponseCode.SubmittingEventIsExist);
		}

		if(CheckEvent.Status == EventStatus.Closed)
		{
			throw new ApiException(ResponseCode.EventIsClosed);
		}	

		// check is max participation
		if (CheckEvent.MaxParticipants == CheckEvent.SubmittingEvents.Count())
		{
			throw new ApiException(ResponseCode.SubmittingEventIsExist);
		}

		//check valid eventifield
		if (request.Answers != null)
		{
			foreach (var anwser in request.Answers)
			{
				var checkField = await _unitOfWork.EventFieldRepsitory.GetAsync(ef => ef.Id == anwser.EventFieldId && ef.EventId == request.EventId);
				if (checkField.Any())
				{
					continue;
				}
				throw new ApiException(ResponseCode.SubmittingEventNotCorrectForm);
			}
		}

		var NewSubmittingEvent = _mapper.Map<SubmittingEvent>(request);
		await _unitOfWork.SubmittingEventRepsitory.AddAsync(NewSubmittingEvent);
		await _unitOfWork.SaveChangesAsync();

		if (request.Answers != null)
		{
			var newAnwsers = request.Answers.Select(a =>
			{
				var field = _unitOfWork.EventFieldRepsitory.Get(f => f.Id == a.EventFieldId).FirstOrDefault();
				var fieldSubmit = _mapper.Map<SubmittingEventField>(field);
				fieldSubmit.SubmittingContent = a.SubmittingContent;
				fieldSubmit.SubmittingEventId = NewSubmittingEvent.Id;
				return fieldSubmit;
			});

			await _unitOfWork.SubmittingEventFieldRepository.AddRange(newAnwsers);
			await _unitOfWork.SaveChangesAsync();
		}
		var GetNewSubmittingEvent = _unitOfWork.SubmittingEventRepsitory.Get(s => s.Id == NewSubmittingEvent.Id)
									.Include(s => s.Event)
									.Include(s => s.SubmittingEventFields)
									.FirstOrDefault();

		var response = _mapper.Map<SubmittingEventResponse>(GetNewSubmittingEvent);

		if (GetNewSubmittingEvent.SubmittingEventFields.Any())
		{
			response.EventFields = GetNewSubmittingEvent.SubmittingEventFields.Select(a => _mapper.Map<EventFieldResponse>(a)).ToList();
		}
		return response;
	}
}
