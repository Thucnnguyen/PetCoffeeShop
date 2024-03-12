

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Memory.Models;
using PetCoffee.Application.Features.Moment.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Moment.Handlers;

public class GetMomentByIdHandler : IRequestHandler<GetMomentByIdQuery, MomentResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public GetMomentByIdHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<MomentResponse> Handle(GetMomentByIdQuery request, CancellationToken cancellationToken)
	{
		var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
		if (currentAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (currentAccount.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}

		if (currentAccount.IsCustomer)
		{
			var MomentsPublic = await _unitOfWork.MomentRepository
							.Get(m => m.Id == request.MomentId)
							.FirstOrDefaultAsync();
			if (MomentsPublic == null)
			{
				throw new ApiException(ResponseCode.MomentNotExisted);
			}
			return _mapper.Map<MomentResponse>(MomentsPublic);
		}

		var Moments = await _unitOfWork.MomentRepository
							.Get(m => m.Id == request.MomentId)
							.FirstOrDefaultAsync();
		if (Moments == null)
		{
			throw new ApiException(ResponseCode.MomentNotExisted);
		}
		 return _mapper.Map<MomentResponse>(Moments);
	}
}
