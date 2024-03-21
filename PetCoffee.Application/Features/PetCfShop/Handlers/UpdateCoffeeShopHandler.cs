using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.PetCfShop.Commands;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using TmsApi.Common;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class UpdateCoffeeShopHandler : IRequestHandler<UpdateCoffeeShopCommand, PetCoffeeShopResponse>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentAccountService _currentAccountService;
    private readonly IAzureService _azureService;

    public UpdateCoffeeShopHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IAzureService azureService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _currentAccountService = currentAccountService;
        _azureService = azureService;
    }

    public async Task<PetCoffeeShopResponse> Handle(UpdateCoffeeShopCommand request, CancellationToken cancellationToken)
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

        if (CurrentAccount.IsCustomer || !CurrentAccount.AccountShops.Any(a => a.ShopId == request.PetCoffeeShopId))
        {
            throw new ApiException(ResponseCode.PermissionDenied);
        };


        var updateShop = await _unitOfWork.PetCoffeeShopRepository.GetByIdAsync(request.PetCoffeeShopId);
        if (updateShop == null)
        {
            throw new ApiException(ResponseCode.ShopNotExisted);
        }

        //assign data
        Assign.Partial(request, updateShop);
        if (request.InstagramUrl == null)
        {
            updateShop.InstagramUrl = "";
        }
        if (request.WebsiteUrl == null)
        {
            updateShop.WebsiteUrl = "";
        }
        if (request.FbUrl == null)
        {
            updateShop.FbUrl = "";
        }
        //upload avatar
        if (request.Avatar != null)
        {
            await _azureService.CreateBlob(request.Avatar.FileName, request.Avatar);
            updateShop.AvatarUrl = await _azureService.GetBlob(request.Avatar.FileName);
        }
        //upload background
        if (request.Background != null)
        {
            await _azureService.CreateBlob(request.Background.FileName, request.Background);
            updateShop.BackgroundUrl = await _azureService.GetBlob(request.Background.FileName);
        }

        await _unitOfWork.PetCoffeeShopRepository.UpdateAsync(updateShop);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PetCoffeeShopResponse>(updateShop);
    }
}
