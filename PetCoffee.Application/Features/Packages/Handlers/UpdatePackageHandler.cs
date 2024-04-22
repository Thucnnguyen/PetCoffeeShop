

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Packages.Commands;
using PetCoffee.Application.Features.Packages.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using TmsApi.Common;

namespace PetCoffee.Application.Features.Packages.Handlers;

public class UpdatePackageHandler : IRequestHandler<UpdatePackageCommand, PackageResponse>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;

	public UpdatePackageHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
	}

	public async Task<PackageResponse> Handle(UpdatePackageCommand request, CancellationToken cancellationToken)
	{
		var isExistedPackage = await _unitOfWork.PackagePromotionRespository
							.Get(p => p.Id != request.Id && (p.Duration == request.Duration || p.Description == request.Description) && !p.Deleted)
							.FirstOrDefaultAsync();
		if (isExistedPackage != null && isExistedPackage.Duration == request.Duration)
		{
			throw new ApiException(ResponseCode.PackageisExisted);
		}
		if(isExistedPackage != null && isExistedPackage.Description == request.Description)
		{
			throw new ApiException(ResponseCode.PackageNameIsExisted);
		}

		var updatedPackage = await _unitOfWork.PackagePromotionRespository
			.Get(pp => pp.Id == request.Id && !pp.Deleted).FirstOrDefaultAsync(); ;

		if (updatedPackage == null)
		{
			throw new ApiException(ResponseCode.PackageNotExist);
		}

		Assign.Partial<UpdatePackageCommand,PackagePromotion>(request,updatedPackage);

		await _unitOfWork.PackagePromotionRespository.UpdateAsync(updatedPackage);
		await _unitOfWork.SaveChangesAsync();

		return _mapper.Map<PackageResponse>(updatedPackage);	

	}

}
