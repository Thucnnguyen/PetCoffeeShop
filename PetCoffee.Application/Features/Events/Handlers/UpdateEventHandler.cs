using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Events.Commands;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using TmsApi.Common;

namespace PetCoffee.Application.Features.Events.Handlers;

public class UpdateEventHandler : IRequestHandler<UpdateEventCommand, EventResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAzureService _azureService;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public UpdateEventHandler(IUnitOfWork unitOfWork, IAzureService azureService, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_azureService = azureService;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<EventResponse> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
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

		if (currentAccount.IsCustomer)
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		};

		var UpdateEvent =  _unitOfWork.EventRepository.Get(e => e.Id == request.Id)
														.Include(e => e.EventFields)
														.FirstOrDefault();
		if(UpdateEvent.StartTime <= DateTime.Now)
		{
			throw new ApiException(ResponseCode.EventCannotChanged);
		}
		if (UpdateEvent == null)
		{
			throw new ApiException(ResponseCode.EventNotExisted);
		}

        if (!currentAccount.AccountShops.Any(a => a.ShopId == UpdateEvent.PetCoffeeShopId) )
        {
			throw new ApiException(ResponseCode.PermissionDenied);
		}

		Assign.Partial(request, UpdateEvent);
		if(request.NewImageFile != null)
		{
			await _azureService.CreateBlob(request.NewImageFile.FileName, request.NewImageFile);
			UpdateEvent.Image = await _azureService.GetBlob(request.NewImageFile.FileName);
		}

		await _unitOfWork.EventRepository.UpdateAsync(UpdateEvent);
		await _unitOfWork.SaveChangesAsync();
		var response = _mapper.Map<EventResponse>(UpdateEvent);
		if (UpdateEvent.EventFields.Any()) 
		{
			response.Fields = UpdateEvent.EventFields.Select(e => _mapper.Map<FieldEventResponseForEventResponse>(e)).ToList();
		}
		return response;
	}
}
