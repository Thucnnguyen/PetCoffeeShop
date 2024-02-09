
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class GetPetCofffeeShopByIdHandler : IRequestHandler<GetPetCoffeeShopByIdQuery, PetCoffeeShopResponse>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public GetPetCofffeeShopByIdHandler(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task<PetCoffeeShopResponse> Handle(GetPetCoffeeShopByIdQuery request, CancellationToken cancellationToken)
	{
		
		var CurrentShop = await _unitOfWork.PetCoffeeShopRepository.GetByIdAsync(request.Id);
		if (CurrentShop == null)
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}

		return _mapper.Map<PetCoffeeShopResponse>(CurrentShop);
	}
}
