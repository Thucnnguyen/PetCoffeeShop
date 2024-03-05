using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Pet.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Pet.Handlers;

public class GetPetsByShopIdHandler : IRequestHandler<GetPetsByShopIdQuery, PaginationResponse<Domain.Entities.Pet, PetResponse>>
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

	public async Task<PaginationResponse<Domain.Entities.Pet, PetResponse>> Handle(GetPetsByShopIdQuery request, CancellationToken cancellationToken)
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
		var PetCoffeeShop = await _unitOfWork.PetCoffeeShopRepository.GetAsync(s => s.Id == request.ShopId);
		if (!PetCoffeeShop.Any())
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}
		var Pets = await _unitOfWork.PetRepository
					.GetAsync(
							predicate: p => p.PetCoffeeShopId == request.ShopId && !p.Deleted,
							includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Pet, object>>>
							{
								p => p.Area
							});
		return new PaginationResponse<Domain.Entities.Pet, PetResponse>(
				Pets,
				request.PageNumber,
				request.PageSize,
				pet => _mapper.Map<PetResponse>(pet));
	}
}
