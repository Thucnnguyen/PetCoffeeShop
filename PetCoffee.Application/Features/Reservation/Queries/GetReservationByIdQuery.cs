using MediatR;
using PetCoffee.Application.Features.Reservation.Models;

namespace PetCoffee.Application.Features.Reservation.Queries
{

	public class GetReservationByIdQuery : IRequest<ReservationDetailResponse>
	{
		public long Id { get; set; }
	}

}
