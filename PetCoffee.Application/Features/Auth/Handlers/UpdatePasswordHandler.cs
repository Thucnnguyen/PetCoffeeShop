

using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class UpdatePasswordHandler : IRequestHandler<UpdatePasswordCommand, AccountResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentAccountService _currentAccountService;

    public UpdatePasswordHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentAccountService = currentAccountService;
    }

    public async Task<AccountResponse> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var CurrentAccount = await _currentAccountService.GetCurrentAccount();
        if (CurrentAccount == null)
        {
            throw new ApiException(ResponseCode.AccountNotExist);
        }
        if (CurrentAccount.IsVerify)
        {
            throw new ApiException(ResponseCode.AccountNotActived);
        }
        // check current password is correct 
        var checkCurPass = HashHelper.CheckHashPwd(request.CurrentPassword, CurrentAccount.Password);
        if (!checkCurPass)
        {
            throw new ApiException(ResponseCode.PassNotValid);
        }

        // hash new account
        CurrentAccount.Password = HashHelper.HashPassword(request.NewPassword);
        await _unitOfWork.AccountRepository.UpdateAsync(CurrentAccount);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<AccountResponse>(CurrentAccount);
    }
}
