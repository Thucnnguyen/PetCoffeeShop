
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Items.Models;
using PetCoffee.Application.Features.Items.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Items.Handlers;

public class GetAllItemInWallethandler : IRequestHandler<GetAllItemInWalletQuery, List<ItemWalletResponse>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public GetAllItemInWallethandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<List<ItemWalletResponse>> Handle(GetAllItemInWalletQuery request, CancellationToken cancellationToken)
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
			return new List<ItemWalletResponse> { };
		}

		var walletItems = await _unitOfWork.WalletItemRepository
					.GetAsync(
						 predicate: wi => wi.WalletId == wallet.Id,
						 includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.WalletItem, object>>>
						 {
							 wi => wi.Item
						 });
		var response = walletItems.Select(wi => _mapper.Map<ItemWalletResponse>(wi)).ToList();

		return response;
	}
}
