
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Packages.Models;
using PetCoffee.Application.Features.Packages.Queries;
using PetCoffee.Application.Persistence.Repository;

namespace PetCoffee.Application.Features.Packages.Handlers;

public class GetPackageByIdHandler : IRequestHandler<GetPackageByIdQuery, PackageResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public GetPackageByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<PackageResponse> Handle(GetPackageByIdQuery request, CancellationToken cancellationToken)
	{
		var package = await _unitOfWork.PackagePromotionRespository.Get(pp => pp.Id == request.Id && !pp.Deleted).FirstOrDefaultAsync();

		if (package == null)
		{
			throw new ApiException(ResponseCode.PackageNotExist);
		}

		return _mapper.Map<PackageResponse>(package);	
	}
}
