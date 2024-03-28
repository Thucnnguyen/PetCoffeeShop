using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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


		var transaction = await _unitOfWork.TransactionRepository
							.Get(t => t.Id == request.TransactionId)
							.Include(t => t.Items)
								.ThenInclude(ti => ti.Item)
							.Include(t => t.Pet)
							.Include(t => t.Reservation)
							.ThenInclude(r => r.Area)
							.ThenInclude(a => a.PetCoffeeShop)
							.Include(t => t.PackagePromotion)
							.Include(t => t.PetCoffeeShop)
							.Include(t => t.CreatedBy)
							.FirstOrDefaultAsync();

		if (transaction == null)
		{
			throw new ApiException(ResponseCode.TransactionNotFound);
		}

		return _mapper.Map<PaymentResponse>(transaction);
	}
}
