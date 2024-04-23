using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Events.Commands;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.Events.Handlers;

public class UpdateEventFieldsHandler : IRequestHandler<UpdateEventFieldsCommand, List<FieldEventResponseForEventResponse>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public UpdateEventFieldsHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<List<FieldEventResponseForEventResponse>> Handle(UpdateEventFieldsCommand request, CancellationToken cancellationToken)
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

		var updateEvent = await _unitOfWork.EventRepository.GetByIdAsync(request.EventId);
		if (updateEvent == null)
		{
			throw new ApiException(ResponseCode.EventNotExisted);
		}

		if (!currentAccount.AccountShops.Any(a => a.ShopId == updateEvent.PetCoffeeShopId))
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		}

		var fieldsExisted = await _unitOfWork.EventFieldRepsitory
							.Get(e => e.EventId == request.EventId)
							.ToListAsync();
		if (fieldsExisted.Any())
		{

			await _unitOfWork.EventFieldRepsitory.DeleteRange(fieldsExisted);
		}

		var NewFieldEvents = request.Fields.Select(f =>
		{
			var newField = _mapper.Map<EventField>(f);
			newField.EventId = updateEvent.Id;
			return newField;
		});
		await _unitOfWork.EventFieldRepsitory.AddRange(NewFieldEvents);
		await _unitOfWork.SaveChangesAsync();

		var response = NewFieldEvents.Select(f => _mapper.Map<FieldEventResponseForEventResponse>(f)).ToList();

		return response;
	}
}
