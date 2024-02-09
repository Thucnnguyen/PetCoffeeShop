using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Pet.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Pet.Handlers;

public class GetPetsByShopIdHandler : IRequestHandler<GetPetsByShopIdQuery, IList<PetResponse>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public GetPetsByShopIdHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<IList<PetResponse>> Handle(GetPetsByShopIdQuery request, CancellationToken cancellationToken)
	{
		var currentAccount = await _currentAccountService.GetCurrentAccount();
		if (currentAccount.PetCoffeeShopId == null)
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}
		var PetCoffeeShop = await _unitOfWork.PetCoffeeShopRepository.GetAsync(s => s.Id == currentAccount.PetCoffeeShopId && s.Status == ShopStatus.Active);
		if (PetCoffeeShop == null)
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}
		var Pets = await _unitOfWork.PetRepository.GetAsync(p => p.PetCoffeeShopId == request.ShopId);
		var response = Pets.Select(p => _mapper.Map<PetResponse>(p)).ToList();	
		return response;
	}
}
