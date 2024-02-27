
	using AutoMapper;
	using MediatR;
	using PetCoffee.Application.Common.Enums;
	using PetCoffee.Application.Common.Exceptions;
	using PetCoffee.Application.Features.Events.Commands;
	using PetCoffee.Application.Features.Events.Models;
	using PetCoffee.Application.Persistence.Repository;
	using PetCoffee.Application.Service;
	using PetCoffee.Domain.Entities;
	using System.Security.Policy;

	namespace PetCoffee.Application.Features.Events.Handlers;


	public class CreateEventFiledHandler : IRequestHandler<CreateEventFieldCommand, List<FieldEventResponseForEventResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentAccountService _currentAccountService;
		private readonly IMapper _mapper;

		public CreateEventFiledHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_currentAccountService = currentAccountService;
			_mapper = mapper;
		}

		public async Task<List<FieldEventResponseForEventResponse>> Handle(CreateEventFieldCommand request, CancellationToken cancellationToken)
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

			if (currentAccount.PetCoffeeShopId == null)
			{
				throw new ApiException(ResponseCode.PermissionDenied);
			}

			var updateEvent = await _unitOfWork.EventRepository.GetByIdAsync(request.EventId);
			if (updateEvent == null)
			{
				throw new ApiException(ResponseCode.EventNotExisted);
			}
			if (updateEvent.PetCoffeeShopId != currentAccount.PetCoffeeShopId)
			{
				throw new ApiException(ResponseCode.PermissionDenied);
			}

			var NewFieldEvents = request.Fields.Select(f =>
			{
				var newField = _mapper.Map<EventField>(f);
				newField.EventId = updateEvent.Id;
				return newField;
			});

			foreach (var fieldEvent in NewFieldEvents)
			{
				
			}
			await _unitOfWork.SaveChangesAsync();

			var response = NewFieldEvents.Select(f => _mapper.Map<FieldEventResponseForEventResponse>(f)).ToList();
			return response;
		}
	}
