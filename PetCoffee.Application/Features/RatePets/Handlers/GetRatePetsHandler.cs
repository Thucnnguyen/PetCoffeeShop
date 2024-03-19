

using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.RatePets.Models;
using PetCoffee.Application.Features.RatePets.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.RatePets.Handlers;

public class GetRatePetsHandlers : IRequestHandler<GetPetRateQuery,  RatePetResponseForCus>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public GetRatePetsHandlers(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<RatePetResponseForCus> Handle(GetPetRateQuery request, CancellationToken cancellationToken)
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

		var petRate = await _unitOfWork.RatePetRespository
					.GetAsync(
						predicate: request.GetExpressions(),
						includes: new List<System.Linq.Expressions.Expression<Func<RatePet, object>>>()
						{
							rp => rp.CreatedBy
						}
					);
		
		var petRateResponse = petRate
							 .Skip((request.PageNumber - 1) * request.PageSize)
							 .Take(request.PageSize)
				             .ToList();
		var response = new RatePetResponseForCus()
		{
			IsRate = petRate.Any(pr => pr.CreatedById == currentAccount.Id),
			RatePets = petRateResponse.Select(pr => _mapper.Map<RatePetResponse>(pr)).ToList()
		};
		return response;
	}
}
