using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Memory.Models;
using PetCoffee.Application.Features.Moment.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;


namespace PetCoffee.Application.Features.Moment.Handlers;

public class GetMomentsByIdHandler : IRequestHandler<GetMomentByPetIdQuery, IList<MomentResponse>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public GetMomentsByIdHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<IList<MomentResponse>> Handle(GetMomentByPetIdQuery request, CancellationToken cancellationToken)
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

		// check view permission
		if (currentAccount.IsCustomer)
		{
			var MomentsPublic = await _unitOfWork.MomentRepository
							.GetAsync(m => m.PetId == request.Id && m.IsPublic == true);
			if (MomentsPublic == null)
			{
				return new List<MomentResponse>();
			}
			return MomentsPublic.Select(m => _mapper.Map<MomentResponse>(m)).ToList();
		}
		   
		var Moments = await _unitOfWork.MomentRepository
							.GetAsync(m => m.PetId == request.Id);
		if (Moments == null)
		{
			return new List<MomentResponse>();
		}
		return Moments.Select(m => _mapper.Map<MomentResponse>(m)).ToList();
	}
}
