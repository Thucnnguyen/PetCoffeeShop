using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Constantsl;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class ResendOTPHandler : IRequestHandler<ResendOTPQuery, bool>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IAzureService _azureService;


	public ResendOTPHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IAzureService azureService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_azureService = azureService;
	}

	public async Task<bool> Handle(ResendOTPQuery request, CancellationToken cancellationToken)
	{
		var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
		if(currentAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (currentAccount.Status != AccountStatus.Verifying)
		{
			throw new ApiException(ResponseCode.AccountIsActived);
		}

		// generate otp again
		currentAccount.OTP = TokenUltils.GenerateOTPCode(6);
		currentAccount.OTPExpired = DateTime.Now.AddDays(1);
		await _unitOfWork.AccountRepository.UpdateAsync(currentAccount);
		await _unitOfWork.SaveChangesAsync();
		//send email
		var EmailContent = string.Format(EmailConstant.EmailForm, currentAccount.FullName, currentAccount.OTP);
		 await _azureService.SendEmail(currentAccount.Email, EmailContent, EmailConstant.EmailSubject);
		return true;
	}
}
