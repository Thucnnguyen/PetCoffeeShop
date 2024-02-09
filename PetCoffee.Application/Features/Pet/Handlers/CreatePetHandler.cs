using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Pet.Commands;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;
using System.Security.Policy;

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
		if(currentAccount.PetCoffeeShopId == null)
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}
		var PetCoffeeShop = await _unitOfWork.PetCoffeeShopRepository.GetAsync(s => s.Id == currentAccount.PetCoffeeShopId && s.Status == ShopStatus.Active);
		if (PetCoffeeShop == null)
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}

		var NewPet = _mapper.Map<Domain.Entities.Pet>(request);
		NewPet.PetCoffeeShopId = currentAccount.PetCoffeeShopId.Value;
		//upload avatar
		if (request.Avatar != null)
		{
			await _azureService.CreateBlob(request.Avatar.FileName, request.Avatar);
			NewPet.Avatar = await _azureService.GetBlob(request.Avatar.FileName);
		}
		//upload avatar
		if (request.Backgound != null)
		{
			await _azureService.CreateBlob(request.Avatar.FileName, request.Avatar);
			NewPet.Backgound = await _azureService.GetBlob(request.Avatar.FileName);
		}

		await _unitOfWork.PetRepository.AddAsync(NewPet);
		await _unitOfWork.SaveChangesAsync();
		var response = _mapper.Map<PetResponse>(NewPet);

		response.CreatedById = currentAccount.Id;
		response.PetCafeShopId = currentAccount.PetCoffeeShopId.Value;

		return response;
	}
}
