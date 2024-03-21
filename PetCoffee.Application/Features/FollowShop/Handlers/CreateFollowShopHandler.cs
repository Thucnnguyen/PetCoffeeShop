

using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.FollowShop.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.FollowShop.Handlers;

internal class CreateFollowShopHandler : IRequestHandler<CreateFollowShopCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentAccountService _currentAccountService;

    public CreateFollowShopHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentAccountService = currentAccountService;
    }

    public async Task<bool> Handle(CreateFollowShopCommand request, CancellationToken cancellationToken)
    {
        var curAccount = await _currentAccountService.GetCurrentAccount();
        if (curAccount == null)
        {
            throw new ApiException(ResponseCode.AccountNotExist);
        }
        if (curAccount.IsVerify)
        {
            throw new ApiException(ResponseCode.AccountNotActived);
        }

        var post = await _unitOfWork.PostRepository.GetByIdAsync(request.PetCoffeeShopId);
        if (post == null)
        {
            throw new ApiException(ResponseCode.PostNotExisted);
        }

        var NewFollowShop = new FollowPetCfShop() { ShopId = request.PetCoffeeShopId };

        await _unitOfWork.FollowPetCfShopRepository.AddAsync(NewFollowShop);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
