
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Moment.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Moment.Handlers;

public class DeleteMomentHandler : IRequestHandler<DeleteMomentCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public DeleteMomentHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<bool> Handle(DeleteMomentCommand request, CancellationToken cancellationToken)
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

		//check moment info
		var DeletedMoment = await _unitOfWork.MomentRepository.Get(m => m.Id == request.Id)
								.Include(m => m.Pet)
								.FirstOrDefaultAsync();
		if (DeletedMoment == null)
		{
			throw new ApiException(ResponseCode.MomentNotExisted);
		}

		//check permission

		if (!currentAccount.AccountShops.Any(a => a.ShopId == DeletedMoment.Pet.PetCoffeeShopId))
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		}
		await _unitOfWork.MomentRepository.DeleteAsync(DeletedMoment);
		await _unitOfWork.SaveChangesAsync();
		return true;
	}
}
