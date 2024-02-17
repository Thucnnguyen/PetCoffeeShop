using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Memory.Commands;
using PetCoffee.Application.Features.Memory.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Memory.Handlers;

public class CreateMomentHandlers : IRequestHandler<CreateMomentCommand, MomentResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAzureService _azureService;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public CreateMomentHandlers(IUnitOfWork unitOfWork, IAzureService azureService, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_azureService = azureService;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<MomentResponse> Handle(CreateMomentCommand request, CancellationToken cancellationToken)
	{
		//get Current account 
		var currentAccount  = await _currentAccountService.GetRequiredCurrentAccount();
		if(currentAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		//check pet info
		var Pet = await _unitOfWork.PetRepository.GetByIdAsync(request.PetId);
		if(Pet == null)
		{
			throw new ApiException(ResponseCode.PetNotExisted);
		}
		//check permission
		if(currentAccount.PetCoffeeShopId == null || currentAccount.PetCoffeeShopId != Pet.PetCoffeeShopId) 
		{
			throw new ApiException(ResponseCode.PermissionDenied);
		}
		
		var NewMemory = _mapper.Map<Domain.Entities.Moment>(request);
		//upload image
		NewMemory.Image = await _azureService.UpdateloadImages(request.Image);
		

		await _unitOfWork.MomentRepository.AddAsync(NewMemory);
		await _unitOfWork.SaveChangesAsync();
		var response = _mapper.Map<MomentResponse>(NewMemory);
		response.CreatedById = currentAccount.Id;
		return response;
	}
}
