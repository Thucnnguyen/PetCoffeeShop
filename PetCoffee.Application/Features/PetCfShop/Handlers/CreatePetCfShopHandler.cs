using AutoMapper;
using MediatR;
using PetCoffee.Application.Features.PetCfShop.Commands;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class CreatePetCfShopHandler : IRequestHandler<CreatePetCfShopCommand, PetCoffeeShopResponse>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IAzureService _azureService;

	public CreatePetCfShopHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IAzureService azureService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_azureService = azureService;
	}

	public async Task<PetCoffeeShopResponse> Handle(CreatePetCfShopCommand request, CancellationToken cancellationToken)
	{
		var CurrentUser = await _currentAccountService.GetCurrentAccount();

		var NewPetCoffeeShop = _mapper.Map<PetCoffeeShop>(request);
		//upload avatar
		if (request.Avatar != null)
		{
			await _azureService.CreateBlob(request.Avatar.FileName, request.Avatar);
			NewPetCoffeeShop.AvatarUrl = await _azureService.GetBlob(request.Avatar.FileName);
		}
		//upload background
		if (request.Background != null)
		{
			await _azureService.CreateBlob(request.Background.FileName, request.Background);
			NewPetCoffeeShop.BackgroundUrl = await _azureService.GetBlob(request.Background.FileName);
		}
		await _unitOfWork.PetCoffeeShopRepository.AddAsync(NewPetCoffeeShop);
		await _unitOfWork.SaveChangesAsync();
		
		CurrentUser.Role = Role.Manager;
		CurrentUser.PetCoffeeShopId = NewPetCoffeeShop.Id;
		await _unitOfWork.AccountRepository.UpdateAsync(CurrentUser);
		await _unitOfWork.SaveChangesAsync();

		var response = _mapper.Map<PetCoffeeShopResponse>(NewPetCoffeeShop);
		response.CreatedById = CurrentUser.Id;
		return response;
	}
}
