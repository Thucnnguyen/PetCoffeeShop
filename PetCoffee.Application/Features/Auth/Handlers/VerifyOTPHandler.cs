
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class VerifyOTPHandler : IRequestHandler<VerifyAccountCommand, bool>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public VerifyOTPHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<bool> Handle(VerifyAccountCommand request, CancellationToken cancellationToken)
	{
		var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
		if (currentAccount.Status != AccountStatus.Verifying)
		{
			throw new ApiException(ResponseCode.AccountIsActived);
		}

		if (currentAccount.IsOTPExpired) 
		{
			throw new ApiException(ResponseCode.OptExpired);
		}

		if(!currentAccount.OTP.Equals(request.OTP)) 
		{
			return false;
		}

		// change status
		currentAccount.Status = AccountStatus.Active;
		//remove otp 
		currentAccount.OTP = "";
		currentAccount.OTPExpired = DateTime.UtcNow.AddDays(-1);
		await _unitOfWork.AccountRepository.UpdateAsync(currentAccount);
		await _unitOfWork.SaveChangesAsync();
		return true;
	}
}
