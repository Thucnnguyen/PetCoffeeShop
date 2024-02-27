using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Events.Commands;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.Events.Handlers;

public class CreateEventHandler : IRequestHandler<CreateEventCommand, EventResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAzureService _azureService;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public CreateEventHandler(IUnitOfWork unitOfWork, IAzureService azureService, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_azureService = azureService;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<EventResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
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

		if(currentAccount.PetCoffeeShopId == null)
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		}

		var NewEvent = _mapper.Map<Event>(request);
		NewEvent.PetCoffeeShopId = currentAccount.PetCoffeeShopId.Value;
		if (request.ImageFile != null)
		{
			await _azureService.CreateBlob(request.ImageFile.FileName, request.ImageFile);
			NewEvent.Image = await _azureService.GetBlob(request.ImageFile.FileName);
		}

		await _unitOfWork.EventRepository.AddAsync(NewEvent);
		await _unitOfWork.SaveChangesAsync();

		var	response = _mapper.Map<EventResponse>(NewEvent);
		return response;
	}
}
