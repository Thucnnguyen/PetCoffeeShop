

using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Events.Commands;
using PetCoffee.Application.Features.SubmitttingEvents.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using TmsApi.Common;

namespace PetCoffee.Application.Features.Events.Handlers;

public class UpdateEventFieldHandler : IRequestHandler<UpdateEventFieldCommand, EventFieldResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public UpdateEventFieldHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<EventFieldResponse> Handle(UpdateEventFieldCommand request, CancellationToken cancellationToken)
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

		if (currentAccount.IsCustomer )
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		};

		var UpdateEventField = await _unitOfWork.EventFieldRepsitory.GetByIdAsync(request.Id);
		if (UpdateEventField == null)
		{
			throw new ApiException(ResponseCode.EventFieldIsNotExist);
		}

		Assign.Partial<UpdateEventFieldCommand, EventField>(request, UpdateEventField);

		await _unitOfWork.EventFieldRepsitory.UpdateAsync(UpdateEventField);
		await _unitOfWork.SaveChangesAsync();

		return _mapper.Map<EventFieldResponse>(UpdateEventField);
	}
}
