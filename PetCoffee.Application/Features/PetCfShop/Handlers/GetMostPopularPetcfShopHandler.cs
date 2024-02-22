
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.PetCfShop.Handlers;

public class GetMostPopularPetcfShopHandler : IRequestHandler<GetAllPetCfShopQuery, PaginationResponse<PetCoffeeShop, PetCoffeeShopResponse>>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public GetMostPopularPetcfShopHandler(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task<PaginationResponse<PetCoffeeShop, PetCoffeeShopResponse>> Handle(GetAllPetCfShopQuery request, CancellationToken cancellationToken)
	{

		var stores = await _unitOfWork.PetCoffeeShopRepository.GetAsync(
			predicate: request.GetExpressions(),
			includes: new List<Expression<Func<PetCoffeeShop, object>>>()
			{
				shop => shop.CreatedBy
			},
			disableTracking: true
		);
		throw new NotImplementedException();
	}
}
