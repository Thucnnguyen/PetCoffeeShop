
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Events.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Events.Handlers;

public class DeleteEventHandler : IRequestHandler<DeleteEventCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public DeleteEventHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<bool> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
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

		if (currentAccount.IsCustomer)
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		};

		var GetEvent = await _unitOfWork.EventRepository
			.Get(s => s.Id == request.EventId && !s.Deleted)
			.Include(e => e.SubmittingEvents)
			.FirstOrDefaultAsync();

		
		if (GetEvent == null)
		{
			throw new ApiException(ResponseCode.EventNotExisted);
		}
		//check permission
		if (!currentAccount.AccountShops.Any(a => a.ShopId == GetEvent.PetCoffeeShopId))
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		}
		//check is event has participation
		if(GetEvent.SubmittingEvents.Any())
		{
			throw new ApiException(ResponseCode.EventCannotDeleted);
		}

		//GetEvent.DeletedAt = DateTime.UtcNow;
		await _unitOfWork.EventRepository.DeleteAsync(GetEvent);
		await _unitOfWork.SaveChangesAsync();
		return true;
	}
}
