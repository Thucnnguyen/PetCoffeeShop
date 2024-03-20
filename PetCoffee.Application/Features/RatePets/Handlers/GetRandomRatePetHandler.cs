

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.RatePets.Models;
using PetCoffee.Application.Features.RatePets.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.RatePets.Handlers;

public class GetRandomRatePetHandler : IRequestHandler<GetRandomRatePetQuery, RatePetResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public GetRandomRatePetHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	async Task<RatePetResponse> IRequestHandler<GetRandomRatePetQuery, RatePetResponse>.Handle(GetRandomRatePetQuery request, CancellationToken cancellationToken)
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
					.Get(
						predicate: rp => rp.PetId == request.PetId,
						includes: new List<System.Linq.Expressions.Expression<Func<RatePet, object>>>()
						{
							rp => rp.CreatedBy
						}
					).ToListAsync();

		var petRateResponse = petRate[new Random().Next(petRate.Count)];
							 
		return _mapper.Map<RatePetResponse>(petRateResponse);
	}
}
