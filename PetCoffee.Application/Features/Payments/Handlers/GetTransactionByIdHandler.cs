using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Payments.Models;
using PetCoffee.Application.Features.Payments.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Payments.Handlers;

public class GetTransactionByIdHandler : IRequestHandler<GetTransactionByIdQuery, PaymentResponse>
{
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public GetTransactionByIdHandler(ICurrentAccountService currentAccountService, IUnitOfWork unitOfWork, IMapper mapper)
	{
		_currentAccountService = currentAccountService;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<PaymentResponse> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
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

		var transaction = await _unitOfWork.TransactionRepository
							.GetByIdAsync(request.TransactionId);
		if (transaction == null)
		{
			throw new ApiException(ResponseCode.TransactionNotFound);
		}

		return _mapper.Map<PaymentResponse>(transaction);
	}
}
