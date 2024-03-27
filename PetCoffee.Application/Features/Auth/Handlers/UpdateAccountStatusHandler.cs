

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System.Security.Cryptography.X509Certificates;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class UpdateAccountStatusHandler : IRequestHandler<UpdateAccountStatusCommand, bool>
{
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public UpdateAccountStatusHandler(ICurrentAccountService currentAccountService, IUnitOfWork unitOfWork, IMapper mapper)
	{
		_currentAccountService = currentAccountService;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<bool> Handle(UpdateAccountStatusCommand request, CancellationToken cancellationToken)
	{
		var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
		if (currentAccount == null)
		{
			throw new ApiException(ResponseCode.Unauthorized);
		}

		if (currentAccount.IsManager)
		{
			var existedStaffAccount = await _unitOfWork.AccountRepository
				.Get(a => a.Id == request.Id && !a.Deleted)
				.Include(a => a.AccountShops)
				.FirstOrDefaultAsync();
			if (existedStaffAccount == null)
			{
				throw new ApiException(ResponseCode.AccountNotExist);
			}
			if (!currentAccount.IsStaff || !currentAccount.AccountShops.Any(acs => acs.ShopId == existedStaffAccount.AccountShops.First().ShopId))
			{
				throw new ApiException(ResponseCode.PermissionDenied);

			}
			existedStaffAccount.Status = request.AccountStatus;
			await _unitOfWork.AccountRepository.UpdateAsync(existedStaffAccount);
			await _unitOfWork.SaveChangesAsync();
			return true;
		}

		var existedAccount = await _unitOfWork.AccountRepository
				.Get(a => a.Id == request.Id && !a.Deleted)
				.Include(a => a.AccountShops)
				.FirstOrDefaultAsync();


		if (existedAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}

		existedAccount.Status = request.AccountStatus;
		await _unitOfWork.AccountRepository.UpdateAsync(existedAccount);
		await _unitOfWork.SaveChangesAsync();
		return true;
	}
}
