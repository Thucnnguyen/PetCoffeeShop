
using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Vaccination.Models;
using PetCoffee.Application.Features.Vaccination.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Vaccination.Handlers;

public class GetVaccinationByIdHandler : IRequestHandler<GetVaccinationByIdQuery, VaccinationResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public GetVaccinationByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_currentAccountService = currentAccountService;
	}

	public async Task<VaccinationResponse> Handle(GetVaccinationByIdQuery request, CancellationToken cancellationToken)
	{
		var CurrentUser = await _currentAccountService.GetCurrentAccount();
		if (CurrentUser == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (CurrentUser.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}
		var Vaccination = await _unitOfWork.VaccinationRepository.GetByIdAsync(request.Id);
		if (Vaccination == null)
		{
			throw new ApiException(ResponseCode.MomentNotExisted);
		}
		return _mapper.Map<VaccinationResponse>(Vaccination);
	}
}
