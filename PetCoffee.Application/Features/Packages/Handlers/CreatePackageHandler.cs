﻿

using AutoMapper;
using MediatR;
using PetCoffee.Application.Features.Packages.Commands;
using PetCoffee.Application.Features.Packages.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.Packages.Handlers;

public class CreatePackageHandler : IRequestHandler<CreatePackageCommand, PackageResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public CreatePackageHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<PackageResponse> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
	{
		var NewPackage = _mapper.Map<PackagePromotion>(request);

		await _unitOfWork.PackagePromotionRespository.AddAsync(NewPackage);
		await _unitOfWork.SaveChangesAsync();

		return _mapper.Map<PackageResponse>(NewPackage);
	}
}
