using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Features.Reservation.Queries;
using PetCoffee.Application.Persistence.Repository;

namespace PetCoffee.Application.Features.Reservation.Handlers
{
	public class GetReservationHandler : IRequestHandler<GetReservationQuery, ReservationDetailResponse>
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IMapper _mapper;

		public GetReservationHandler(IMapper mapper, IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}
		public async Task<ReservationDetailResponse> Handle(GetReservationQuery request, CancellationToken cancellationToken)
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



			return _mapper.Map<ReservationDetailResponse>(order);


			return _mapper.Map<ReservationDetailResponse>(order);



		}
	}
}
