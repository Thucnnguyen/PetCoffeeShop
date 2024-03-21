

using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Packages.Models;
using PetCoffee.Application.Features.Packages.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Packages.Handlers;

public class GetAllPackageHandler : IRequestHandler<GetAllPackageQuery, List<PackageResponse>>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public GetAllPackageHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<List<PackageResponse>> Handle(GetAllPackageQuery request, CancellationToken cancellationToken)
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

		var allPackage = await _unitOfWork.PackagePromotionRespository.GetAsync();

		return allPackage.Select(p => _mapper.Map<PackageResponse>(p)).ToList();
	}
}
