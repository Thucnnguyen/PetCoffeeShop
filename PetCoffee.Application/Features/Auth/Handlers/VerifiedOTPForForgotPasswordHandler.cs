
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class VerifiedOTPForForgotPasswordHandler : IRequestHandler<VerifiedOTPForForgotPasswordCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentAccountService _currentAccountService;

    public VerifiedOTPForForgotPasswordHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _currentAccountService = currentAccountService;
    }

    public async Task<bool> Handle(VerifiedOTPForForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var currentAccount = (await _unitOfWork.AccountRepository.GetAsync(a => a.Email == request.Email)).First();

        if (currentAccount == null)
        {
            throw new ApiException(ResponseCode.AccountNotExist);
        }

        if (currentAccount.IsOTPExpired)
        {
            throw new ApiException(ResponseCode.OptExpired);
        }

        if (!currentAccount.OTP.Equals(request.OTP))
        {
            return false;
        }

        //remove otp 
        currentAccount.OTP = "";
        currentAccount.OTPExpired = DateTime.Now.AddDays(-1);
        await _unitOfWork.AccountRepository.UpdateAsync(currentAccount);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
