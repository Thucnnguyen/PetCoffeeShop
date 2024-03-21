
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class RegisterShopStaffAccountHandler : IRequestHandler<RegisterShopStaffAccountCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentAccountService _currentAccountService;
    private readonly IMapper _mapper;

    public RegisterShopStaffAccountHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentAccountService = currentAccountService;
        _mapper = mapper;
    }

    public async Task<bool> Handle(RegisterShopStaffAccountCommand request, CancellationToken cancellationToken)
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

        if (!currentAccount.AccountShops.Any(a => a.ShopId == request.ShopId))
        {
            throw new ApiException(ResponseCode.PermissionDenied);
        };

        var checkEmail = await _unitOfWork.AccountRepository.GetAsync(a => a.Email == request.Email && !a.Deleted);
        if (checkEmail.Any())
        {
            throw new ApiException(ResponseCode.AccountIsExisted);
        }

        var shop = await _unitOfWork.PetCoffeeShopRepository.GetByIdAsync(request.ShopId);
        if (shop == null)
        {
            throw new ApiException(ResponseCode.ShopNotExisted);
        }

        var newStaffAccount = _mapper.Map<Account>(request);
        newStaffAccount.Password = HashHelper.HashPassword(request.Password);
        newStaffAccount.Role = Role.Staff;
        newStaffAccount.LoginMethod = LoginMethod.UserNamePass;
        newStaffAccount.Status = AccountStatus.Active;
        newStaffAccount.Avatar = shop.AvatarUrl;
        newStaffAccount.Background = shop.BackgroundUrl;
        newStaffAccount.PhoneNumber = shop.Phone;

        await _unitOfWork.AccountRepository.AddAsync(newStaffAccount);
        await _unitOfWork.SaveChangesAsync();

        var newAccountShop = new AccountShop()
        {
            AccountId = newStaffAccount.Id,
            ShopId = shop.Id,

        };
        await _unitOfWork.AccountShopRespository.AddAsync(newAccountShop);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
