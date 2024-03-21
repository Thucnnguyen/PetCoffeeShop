using MediatR;
using PetCoffee.Application.Features.Reservation.Models;

namespace PetCoffee.Application.Features.Reservation.Queries
{
	public class GetReservationByAreaIdQuery : IRequest<IList<ReservationResponse>>
	{
		public long AreaId { get; set; }
	}
}
