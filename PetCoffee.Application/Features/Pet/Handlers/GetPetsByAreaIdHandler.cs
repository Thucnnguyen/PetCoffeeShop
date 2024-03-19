﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Pet.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Pet.Handlers;

public class GetPetsByAreaIdHandler : IRequestHandler<GetPetsByAreaIdQuery, PaginationResponse<Domain.Entities.Pet, PetResponse>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public GetPetsByAreaIdHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<PaginationResponse<Domain.Entities.Pet, PetResponse>> Handle(GetPetsByAreaIdQuery request, CancellationToken cancellationToken)
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
		var area = await _unitOfWork.AreaRepsitory.GetAsync(s => s.Id == request.AreaId);
		if (!area.Any())
		{
			throw new ApiException(ResponseCode.ShopNotExisted);
		}

		var Pets =  _unitOfWork.PetRepository
					.Get(predicate: request.GetExpressions())
					.Include(p => p.PetAreas)
					.ThenInclude(pa => pa.Area)
					.Include(p => p.PetRattings)
					.AsQueryable();

		return new PaginationResponse<Domain.Entities.Pet, PetResponse>(
				Pets,
				request.PageNumber,
				request.PageSize,
				pet => _mapper.Map<PetResponse>(pet));
	}
}
