using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Product.Models;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Features.Reservation.Queries;
using PetCoffee.Application.Persistence.Repository;

namespace PetCoffee.Application.Features.Reservation.Handlers
{
	public class GetReservationByIdHandler : IRequestHandler<GetReservationByIdQuery, ReservationDetailResponse>
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IMapper _mapper;

		public GetReservationByIdHandler(IMapper mapper, IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}
		public async Task<ReservationDetailResponse> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
		{


			var orderQuery = await _unitOfWork.ReservationRepository.GetAsync(
		 predicate: order => order.Id == request.Id,
		 disableTracking: true);
			var order = await orderQuery

				.Include(order => order.Transactions).FirstOrDefaultAsync(cancellationToken);

			if (order == null)
			{
				throw new ApiException(ResponseCode.ReservationNotExist);
			}

			var products = await _unitOfWork.ReservationProductRepository
				.Get(rp => rp.ReservationId == request.Id)
				.Include(rp => rp.Product).ToListAsync();
				
			var response = _mapper.Map<ReservationDetailResponse>(order);
			response.Products = products.Select(p => _mapper.Map<ProductForReservationResponse>(p)).ToList();
			return response;
		}
	}
}
