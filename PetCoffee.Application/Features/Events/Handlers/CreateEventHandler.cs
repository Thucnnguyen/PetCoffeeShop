﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Events.Commands;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Application.Service.Notifications;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Events.Handlers;

public class CreateEventHandler : IRequestHandler<CreateEventCommand, EventResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAzureService _azureService;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;
	private readonly INotifier _notifier;
	private readonly ISchedulerService _schedulerService;

	public CreateEventHandler(IUnitOfWork unitOfWork, IAzureService azureService, ICurrentAccountService currentAccountService, IMapper mapper, INotifier notifier, ISchedulerService schedulerService)
	{
		_unitOfWork = unitOfWork;
		_azureService = azureService;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
		_notifier = notifier;
		_schedulerService = schedulerService;
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

		if (currentAccount.IsCustomer || !currentAccount.AccountShops.Any(a => a.ShopId == request.PetCoffeeShopId))
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		}
		var shop = await _unitOfWork.PetCoffeeShopRepository.Get(s => s.Id == request.PetCoffeeShopId && !s.Deleted).FirstOrDefaultAsync();
		if(shop == null)
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}

		var NewEvent = _mapper.Map<Event>(request);
		if (request.ImageFile != null)
		{
			await _azureService.CreateBlob(request.ImageFile.FileName, request.ImageFile);
			NewEvent.Image = await _azureService.GetBlob(request.ImageFile.FileName);
		}

		await _unitOfWork.EventRepository.AddAsync(NewEvent);
		await _unitOfWork.SaveChangesAsync();

		var response = _mapper.Map<EventResponse>(NewEvent);
		NewEvent.CreatedBy = currentAccount;
		NewEvent.PetCoffeeShop = shop;

		var follows = await _unitOfWork.FollowPetCfShopRepository.Get(a => a.ShopId == NewEvent.PetCoffeeShopId)
																	.Include(a => a.CreatedBy)
																	.ToListAsync();
		foreach (var follow in follows)
		{
			var notification = new Notification(
				account: follow.CreatedBy,
				type: NotificationType.NewEvent,
				entityType: EntityType.Event,
				data: NewEvent,
				shopId: NewEvent.PetCoffeeShopId
			);
			await _notifier.NotifyAsync(notification, true);
		}

		// set schedule to check total participants
		var startTimeParts = NewEvent.StartTime.Split(":");
		var CheckDate = new DateTimeOffset(
			NewEvent.StartDate.Year,
			NewEvent.StartDate.Month,
			NewEvent.StartDate.Day,
			int.Parse(startTimeParts[0]),
			int.Parse(startTimeParts[1]),
			0,
			DateTimeOffset.UtcNow.Offset
		);
		await _schedulerService.CheckEventHasEnoughParticipantJob(NewEvent.Id, CheckDate.AddDays(-1));
		return response;
	}
}
