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


			var order = await _unitOfWork.ReservationRepository.Get(
							 predicate: order => order.Id == request.Id,
							 includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Reservation, object>>>
							 {
								 p => p.CreatedBy,
							 },
							 disableTracking: true)
							.FirstOrDefaultAsync();
			

			if (order == null)
			{
				throw new ApiException(ResponseCode.ReservationNotExist);
			}

			var products = await _unitOfWork.ReservationProductRepository
				.Get(rp => rp.ReservationId == request.Id)
				.Include(rp => rp.Product).ToListAsync();
				
			var response = _mapper.Map<ReservationDetailResponse>(order);
			response.AccountForReservation = _mapper.Map<AccountForReservation>(order.CreatedBy);
			response.Products = products.Select(p => _mapper.Map<ProductForReservationResponse>(p)).ToList();
			return response;
		}
	}
}
