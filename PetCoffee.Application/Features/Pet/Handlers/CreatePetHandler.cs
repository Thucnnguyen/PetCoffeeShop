﻿using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Pet.Commands;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Pet.Handlers;

public class CreatePetHandler : IRequestHandler<CreatePetCommand, PetResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAzureService _azureService;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public CreatePetHandler(IUnitOfWork unitOfWork, IAzureService azureService, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_azureService = azureService;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<PetResponse> Handle(CreatePetCommand request, CancellationToken cancellationToken)
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

		if (!currentAccount.AccountShops.Any(a => a.ShopId == request.PetCoffeeShopId))
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		};

		var PetCoffeeShop = await _unitOfWork.PetCoffeeShopRepository.GetAsync(s => s.Id == request.PetCoffeeShopId && s.Status == ShopStatus.Active);
		if (PetCoffeeShop == null)
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}


		var NewPet = _mapper.Map<Domain.Entities.Pet>(request);
		//upload avatar
		if (request.Avatar != null)
		{
			await _azureService.CreateBlob(request.Avatar.FileName, request.Avatar);
			NewPet.Avatar = await _azureService.GetBlob(request.Avatar.FileName);
		}
		//upload Backgrounds
		if (request.Backgrounds != null)
		{
			NewPet.Backgound = await _azureService.UpdateloadImages(request.Backgrounds);
		}

		await _unitOfWork.PetRepository.AddAsync(NewPet);
		await _unitOfWork.SaveChangesAsync();
		var response = _mapper.Map<PetResponse>(NewPet);

		response.CreatedById = currentAccount.Id;

		return response;
	}
}
