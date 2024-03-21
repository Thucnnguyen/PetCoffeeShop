
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Transactions.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.Transactions.Handlers;

public class BuyItemHandler : IRequestHandler<BuyItemsCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentAccountService _currentAccountService;
    private readonly IMapper _mapper;

    public BuyItemHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentAccountService = currentAccountService;
        _mapper = mapper;
    }

    public async Task<bool> Handle(BuyItemsCommand request, CancellationToken cancellationToken)
    {
        var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
        if (currentAccount == null)
        {
            throw new ApiException(ResponseCode.AccountNotExist);
        }
        if (currentAccount.IsVerify)
        {
            throw new ApiException(ResponseCode.AccountNotActived);
        }

        var items = new List<Item>();
        var itemDic = new Dictionary<long, int>();
        foreach (var item in request.Items)
        {
            var existedItem = await _unitOfWork.ItemRepository.GetByIdAsync(item.ItemId);
            if (existedItem == null)
            {
                throw new ApiException(ResponseCode.ItemNotExist);
            }
            items.Add(existedItem);
            itemDic.Add(item.ItemId, item.Quantity);
        }

        var wallet = await _unitOfWork.WalletRepsitory.Get(w => w.CreatedById == currentAccount.Id)
                    .FirstOrDefaultAsync();
        if (wallet == null)
        {
            throw new ApiException(ResponseCode.NotEnoughBalance);
        }

        double totalMoney = 0;
        foreach (var item in items)
        {
            totalMoney += item.Price * itemDic[item.ItemId];
        }

        if ((decimal)totalMoney > wallet.Balance)
        {
            throw new ApiException(ResponseCode.NotEnoughBalance);
        }

        wallet.Balance -= (decimal)totalMoney;
        foreach (var item in items)
        {
            var itemWallet = await _unitOfWork.WalletItemRepository
                                .Get(iw => iw.WalletId == wallet.Id && iw.ItemId == item.ItemId)
                                .FirstOrDefaultAsync();
            if (itemWallet == null)
            {
                await _unitOfWork.WalletItemRepository.AddAsync(new WalletItem()
                {
                    ItemId = item.ItemId,
                    WalletId = wallet.Id,
                    TotalItem = itemDic[item.ItemId]
                });
                await _unitOfWork.SaveChangesAsync();
                continue;
            }

            itemWallet.TotalItem += itemDic[item.ItemId];
            await _unitOfWork.WalletItemRepository.UpdateAsync(itemWallet);
            await _unitOfWork.SaveChangesAsync();
        }

        return true;
    }
}
