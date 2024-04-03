

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Items.Models;
using PetCoffee.Application.Features.Transactions.Models;
using PetCoffee.Application.Features.Wallets.Models;
using PetCoffee.Application.Features.Wallets.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Wallets.Handlers;

public class GetWalletForCurrentAccountHandler : IRequestHandler<GetWalletForCurrentAccountQuery, WalletResponse>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public GetWalletForCurrentAccountHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<WalletResponse> Handle(GetWalletForCurrentAccountQuery request, CancellationToken cancellationToken)
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

		var wallet = await _unitOfWork.WalletRepsitory.Get(w => w.CreatedById == currentAccount.Id)
						.FirstOrDefaultAsync();
		if (wallet == null)
		{
			return new WalletResponse() { Balance = 0 };
		}
		var walletResponse = _mapper.Map<WalletResponse>(wallet);
		var walletItems = await _unitOfWork.WalletItemRepository
					.GetAsync(
						 predicate: wi => wi.WalletId == wallet.Id,
						 includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.WalletItem, object>>>
						 {
							 wi => wi.Item,
						 });
		walletResponse.Items = walletItems.Select(wi => _mapper.Map<ItemWalletResponse>(wi)).ToList();

		var transactions = _unitOfWork.TransactionRepository
								.Get(Transaction => Transaction.Remitter.CreatedById == currentAccount.Id ||
												Transaction.CreatedById == currentAccount.Id ||
												Transaction.Wallet.CreatedById == currentAccount.Id)
								.Include(t => t.Items)
								.ThenInclude(ti => ti.Item)
							.Include(t => t.Pet)
							.Include(t => t.Reservation)
							.ThenInclude(r => r.Area)
							.ThenInclude(a => a.PetCoffeeShop)
							.Include(t => t.PackagePromotion)
							.Include(t => t.PetCoffeeShop)
							.Include(t => t.CreatedBy)
								.OrderByDescending(tr => tr.CreatedAt);
		walletResponse.Transactions = transactions.Select(tr => _mapper.Map<TransactionResponse>(tr)).ToList();
		return walletResponse;
	}
}
