

using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Constantsl;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class SendOTPForForgotPasswordHandler : IRequestHandler<SendOTPForForgotPasswordCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAzureService _azureService;
    private readonly ICurrentAccountService _currentAccountService;
    private readonly IMapper _mapper;

    public SendOTPForForgotPasswordHandler(IUnitOfWork unitOfWork, IAzureService azureService, IMapper mapper, ICurrentAccountService currentAccountService)
    {
        _unitOfWork = unitOfWork;
        _azureService = azureService;
        _mapper = mapper;
        _currentAccountService = currentAccountService;
    }

    public async Task<bool> Handle(SendOTPForForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var AccountIsExist = await _unitOfWork.AccountRepository.GetAsync(a => a.Email == request.Email);

        if (AccountIsExist == null || !AccountIsExist.Any())
        {
            throw new ApiException(ResponseCode.AccountNotExist);
        }
        // generate otp again
        var currentAccount = AccountIsExist.First();
        currentAccount.OTP = TokenUltils.GenerateOTPCode(6);
        currentAccount.OTPExpired = DateTime.Now.AddDays(1);
        await _unitOfWork.AccountRepository.UpdateAsync(currentAccount);
        await _unitOfWork.SaveChangesAsync();

        var EmailContent = string.Format(EmailConstant.EmailOTPForm, currentAccount.FullName, currentAccount.OTP);
        await _azureService.SendEmail(currentAccount.Email, EmailContent, EmailConstant.EmailSubject);
        return true;
    }
}
