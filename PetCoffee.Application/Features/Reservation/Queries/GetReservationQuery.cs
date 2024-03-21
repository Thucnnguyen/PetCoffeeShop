using MediatR;
using PetCoffee.Application.Features.Reservation.Models;

namespace PetCoffee.Application.Features.Reservation.Queries
{

	public class GetReservationQuery : IRequest<ReservationDetailResponse>
	{
		public long Id { get; set; }
	}

}
