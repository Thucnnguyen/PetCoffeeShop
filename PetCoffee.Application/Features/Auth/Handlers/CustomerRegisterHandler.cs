using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Constantsl;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class CustomerRegisterHandler : IRequestHandler<CustomerRegisterCommand, AccessTokenResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAzureService _azureService;
	private readonly IJwtService _jwtService;
	private readonly IMapper _mapper;
	private readonly ISchedulerService _schedulerService;

	public CustomerRegisterHandler(IUnitOfWork unitOfWork, IJwtService jwtService, IMapper mapper, IAzureService azureService, ISchedulerService schedulerService)
	{
		_unitOfWork = unitOfWork;
		_jwtService = jwtService;
		_mapper = mapper;
		_azureService = azureService;
		_schedulerService = schedulerService;
	}
	public async Task<AccessTokenResponse> Handle(CustomerRegisterCommand request, CancellationToken cancellationToken)
	{
		var isExisted = _unitOfWork.AccountRepository.IsExisted(a => a.Email.Equals(request.Email));
		if(isExisted)
		{
			throw new ApiException(ResponseCode.AccountIsExisted);
		}
		// hash password
		var hasPassword = HashHelper.HashPassword(request.Password);
		var account = _mapper.Map<Account>(request);

		if (request.Avatar != null)
		{
			await _azureService.CreateBlob(request.Avatar.FileName, request.Avatar);
			account.Avatar = await _azureService.GetBlob(request.Avatar.FileName);
		}
		if (request.Background != null)
		{
			await _azureService.CreateBlob(request.Background.FileName, request.Background);
			account.Background = await _azureService.GetBlob(request.Background.FileName);
		}

		account.Password = hasPassword;
		account.Role = Role.Customer;
		account.LoginMethod = LoginMethod.UserNamePass;
		account.OTP = TokenUltils.GenerateOTPCode(6);
		account.OTPExpired = DateTime.UtcNow.AddDays(1);

		var newAccount = await _unitOfWork.AccountRepository.AddAsync(account);
		await _unitOfWork.SaveChangesAsync();
		//send email
		var EmailContent = string.Format(EmailConstant.EmailForm, account.FullName, account.OTP);
		await _azureService.SendEmail(account.Email, EmailContent, EmailConstant.EmailSubject);
		await _schedulerService.DeleteAccountNotVerify(account.Id,account.CreatedAt.AddDays(2));
		var resp = new AccessTokenResponse(_jwtService.GenerateJwtToken(newAccount));
		return resp;
	}
}
