using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.Payments.Models;
using PetCoffee.Application.Features.Payments.Queries;
using PetCoffee.Application.Features.Product.Models;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Payments.Handlers;

public class GetAllTransactionHandler : IRequestHandler<GetAllTransactionByCustomerIdOrShopIdQuery, PaginationResponse<Domain.Entities.Transaction, PaymentResponse>>
{
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public GetAllTransactionHandler(ICurrentAccountService currentAccountService, IUnitOfWork unitOfWork, IMapper mapper)
	{
		_currentAccountService = currentAccountService;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<PaginationResponse<Domain.Entities.Transaction, PaymentResponse>> Handle(GetAllTransactionByCustomerIdOrShopIdQuery request, CancellationToken cancellationToken)
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

		if (currentAccount != null && currentAccount.IsCustomer && request.ShopId != 0)
		{
			request.CustomerId = currentAccount.Id;
		}

		var payments = await _unitOfWork.TransactionRepository
		   .Get(
			   predicate: request.GetExpressions(),
			   orderBy: request.GetOrder(),
			   disableTracking: true)
							.Include(t => t.Items)
								.ThenInclude(ti => ti.Item)
							.Include(t => t.Pet)
							.Include(t => t.Reservation)
							.ThenInclude(r => r.Area)
							.ThenInclude(a => a.PetCoffeeShop)
							.Include(t => t.PackagePromotion)
							.Include(t => t.PetCoffeeShop)
							.Include(t => t.CreatedBy)
							.Include(r => r.Reservation)
								.ThenInclude(r => r.Area)
							.ToListAsync();
		var paymentsResponse = payments
							.Skip((request.PageNumber - 1) * request.PageSize)
							.Take(request.PageSize);
		var response = new List<PaymentResponse>();
		foreach ( var item in payments )
		{
			var payment = _mapper.Map<PaymentResponse>(item);
			if (item.Reservation != null)
			{
				payment.Reservation = _mapper.Map<ReservationResponse>(item.Reservation);
				payment.Reservation.AreaResponse = _mapper.Map<AreaResponse>(item.Reservation.Area);

				var products = await _unitOfWork.ReservationProductRepository
					.Get(rp => rp.ReservationId == payment.ReservationId)
					.Include(rp => rp.Product).ToListAsync();
				payment.Reservation.Products = products.Select(p => _mapper.Map<ProductForReservationResponse>(p)).ToList();

			}
			response.Add(payment);

		}

		return new PaginationResponse<Domain.Entities.Transaction, PaymentResponse>(
			response,
			payments.Count(),
			request.PageNumber,
			request.PageSize);
	}
}
