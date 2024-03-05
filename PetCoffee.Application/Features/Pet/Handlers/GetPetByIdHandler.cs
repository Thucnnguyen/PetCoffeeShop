

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
	private readonly ICurrentAccountService _currentAccountService;

	public GetPetByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_currentAccountService = currentAccountService;
	}

	public async Task<PetResponse> Handle(GetPetByIdQuery request, CancellationToken cancellationToken)
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

		var Pet = (await _unitOfWork.PetRepository
				.GetAsync(
						predicate: p => p.Id == request.Id && !p.Deleted,
						includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Pet, object>>>
						{
							p => p.Area
						})
				).FirstOrDefault();
		

		if (Pet == null) 
		{
			throw new ApiException(ResponseCode.PetNotExisted);
		}
		return _mapper.Map<PetResponse>(Pet);
	}
}
