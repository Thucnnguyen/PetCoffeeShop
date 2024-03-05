

using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Features.Auth.Queries;
using PetCoffee.Application.Persistence.Repository;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class GetAccountInformationHandler : IRequestHandler<GetAccountInformationByIdQuery, AccountResponse>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public GetAccountInformationHandler(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task<AccountResponse> Handle(GetAccountInformationByIdQuery request, CancellationToken cancellationToken)
	{
		var currentAccount = await _unitOfWork.AccountRepository.GetByIdAsync(request.Id);
		if (currentAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (currentAccount.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}

		var response = _mapper.Map<AccountResponse>(currentAccount);
		return response;
	}
}
