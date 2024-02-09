

using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Pet.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Pet.Handlers;

internal class GetPetByIdHandler : IRequestHandler<GetPetByIdQuery, PetResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public GetPetByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<PetResponse> Handle(GetPetByIdQuery request, CancellationToken cancellationToken)
	{
		var Pet = await _unitOfWork.PetRepository.GetByIdAsync(request.Id);
		
		if (Pet == null) 
		{
			throw new ApiException(ResponseCode.PetNotExisted);
		}
		return _mapper.Map<PetResponse>(Pet);
	}
}
