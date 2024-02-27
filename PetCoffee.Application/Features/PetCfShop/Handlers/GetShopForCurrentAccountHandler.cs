
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class GetShopForCurrentAccountHandler : IRequestHandler<GetPetCfShopForCurrentAccountQuery, PetCoffeeShopResponse>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentAccountService _currentAccountService;

	public GetShopForCurrentAccountHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<PetCoffeeShopResponse> Handle(GetPetCfShopForCurrentAccountQuery request, CancellationToken cancellationToken)
    {
        var CurrentAccount = await _currentAccountService.GetCurrentAccount();
        if (CurrentAccount == null)
        {
            throw new ApiException(ResponseCode.AccountNotExist);
        }

        if (CurrentAccount.PetCoffeeShopId == null)
        {
            throw new ApiException(ResponseCode.ShopNotExisted);
        }

        var CurrentShop = await _unitOfWork.PetCoffeeShopRepository.Get(p => p.Id == CurrentAccount.PetCoffeeShopId)
                                                                    .Include(p => p.Follows)
                                                                    .FirstOrDefaultAsync();
        if (CurrentShop == null)
        {
            throw new ApiException(ResponseCode.ShopNotExisted);
        }

        var response = _mapper.Map<PetCoffeeShopResponse>(CurrentShop);
        response.TotalFollow = CurrentShop.Follows.Count();

		return response;
    }
}
