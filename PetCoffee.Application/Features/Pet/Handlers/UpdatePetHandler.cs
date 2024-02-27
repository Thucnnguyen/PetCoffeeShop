
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Pet.Commands;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using TmsApi.Common;

namespace PetCoffee.Application.Features.Pet.Handlers;

public class UpdatePetHandler : IRequestHandler<UpdatePetCommand, PetResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAzureService _azureService;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public UpdatePetHandler(IUnitOfWork unitOfWork, IAzureService azureService, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_azureService = azureService;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<PetResponse> Handle(UpdatePetCommand request, CancellationToken cancellationToken)
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
		if (currentAccount.PetCoffeeShopId == null)
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		}

		var pet = (await _unitOfWork.PetRepository.GetAsync(p => p.Id == request.Id && !p.Deleted)).FirstOrDefault();

		if (pet == null)
		{
			throw new ApiException(ResponseCode.PetNotExisted);
		}
		if (currentAccount.PetCoffeeShopId != pet.PetCoffeeShopId)
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		}

		Assign.Partial<UpdatePetCommand, Domain.Entities.Pet>(request, pet);
		//upload new avatar
		if (request.NewAvatar != null)
		{
			await _azureService.CreateBlob(request.NewAvatar.FileName, request.NewAvatar);
			pet.Avatar = await _azureService.GetBlob(request.NewAvatar.FileName);
		}
		//upload Backgrounds
		if (request.NewBackgrounds != null)
		{
			pet.Backgound = await _azureService.UpdateloadImages(request.NewBackgrounds);
		}

		await _unitOfWork.PetRepository.UpdateAsync(pet);
		await _unitOfWork.SaveChangesAsync();

		var response = _mapper.Map<PetResponse>(pet);

		return response;
	}
}
