﻿

using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Pet.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Pet.Handlers;

public class DeletePetHandler : IRequestHandler<DeletePetCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly ICacheService _cacheService;

	public DeletePetHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService, ICacheService cacheService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_currentAccountService = currentAccountService;
		_cacheService = cacheService;
	}

	public async Task<bool> Handle(DeletePetCommand request, CancellationToken cancellationToken)
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

		if (currentAccount.IsCustomer)
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		};

		var pet = (await _unitOfWork.PetRepository.GetAsync(p => p.Id == request.Id && !p.Deleted)).FirstOrDefault();

		if (pet == null)
		{
			throw new ApiException(ResponseCode.PetNotExisted);
		}

		if (!currentAccount.AccountShops.Any(a => a.ShopId == pet.PetCoffeeShopId))
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		}

		pet.DeletedAt = DateTime.UtcNow;

		await _unitOfWork.PetRepository.UpdateAsync(pet);
		await _unitOfWork.SaveChangesAsync();

		await _cacheService.RemoveAsync(pet.Id.ToString(), cancellationToken);
		return true;

	}
}
