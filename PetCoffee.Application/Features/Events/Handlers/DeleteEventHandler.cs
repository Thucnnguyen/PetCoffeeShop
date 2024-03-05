
using AutoMapper;
using MediatR;
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

		if (currentAccount.IsCustomer )
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		};

		var GetEvent = ( await _unitOfWork.EventRepository.GetAsync(s => s.Id == request.EventId && !s.Deleted))
			.FirstOrDefault();

		if (GetEvent == null)
		{
			throw new ApiException(ResponseCode.EventNotExisted);
		}
		if( !currentAccount.AccountShops.Any(a => a.ShopId == GetEvent.PetCoffeeShopId))
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		}

		GetEvent.DeletedAt = DateTime.Now;
		await _unitOfWork.EventRepository.UpdateAsync(GetEvent);
		await _unitOfWork.SaveChangesAsync();
		return true;
	}
}
