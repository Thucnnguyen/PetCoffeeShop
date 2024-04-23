

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class ChangeStaffPasswordHandler : IRequestHandler<ChangeStaffPasswordCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public ChangeStaffPasswordHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_currentAccountService = currentAccountService;
	}

	public async Task<bool> Handle(ChangeStaffPasswordCommand request, CancellationToken cancellationToken)
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
		var shopIds= currentAccount.AccountShops.Select(s => s.ShopId).ToList();
		var staff = await _unitOfWork.AccountRepository
							.Get(a => a.Id == request.Id 
							&& a.AccountShops.Any(acs => shopIds.Contains(acs.ShopId))
							&& !a.Deleted)
							.FirstOrDefaultAsync();	
		if (staff == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}

		staff.Password = HashHelper.HashPassword(request.NewPassword);
		
		await _unitOfWork.AccountRepository.UpdateAsync(staff);
		await _unitOfWork.SaveChangesAsync();

		return true;
	}
}
