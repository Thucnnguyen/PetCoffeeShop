
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.FollowShop.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.FollowShop.Handlers;

public class DeleteFollowShopHandler : IRequestHandler<DeleteFollowShopCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly ICurrentAccountService _currentAccountService;

	public DeleteFollowShopHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_currentAccountService = currentAccountService;
	}

	public async Task<bool> Handle(DeleteFollowShopCommand request, CancellationToken cancellationToken)
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

		var followShop = (await _unitOfWork.FollowPetCfShopRepository.GetAsync(f => f.ShopId == request.PetCoffeeShopId && f.CreatedById == curAccount.Id)).FirstOrDefault();
		if (followShop == null)
		{
			throw new ApiException(ResponseCode.FollowNotExisted);
		}
		await _unitOfWork.FollowPetCfShopRepository.DeleteAsync(followShop);
		await _unitOfWork.SaveChangesAsync();
		return true;
	}
}
