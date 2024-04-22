using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.Payments.Models;
using PetCoffee.Application.Features.Payments.Queries;
using PetCoffee.Application.Features.Product.Models;
using PetCoffee.Application.Features.Reservation.Models;
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
							.Include(t => t.Reservation)
								.ThenInclude(r => r.Area)
							.Include(t => t.TransactionProducts)
							.FirstOrDefaultAsync();

		if (transaction == null)
		{
			throw new ApiException(ResponseCode.TransactionNotFound);
		}
		var response = _mapper.Map<PaymentResponse>(transaction);

		if (transaction.Reservation != null)
		{
			response.Reservation = _mapper.Map<ReservationResponse>(transaction.Reservation);
			response.Reservation.AreaResponse = _mapper.Map<AreaResponse>(transaction.Reservation.Area);

			var products = await _unitOfWork.ReservationProductRepository
				.Get(rp => rp.ReservationId == transaction.ReservationId)
				.Include(rp => rp.Product).ToListAsync();
			response.Reservation.Products = products.Select(p => _mapper.Map<ProductForReservationResponse>(p)).ToList();
		}
		
		
		return response;
	}
}
