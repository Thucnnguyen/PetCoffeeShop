using MediatR;
using PetCoffee.Application.Features.Reservation.Models;

namespace PetCoffee.Application.Features.Reservation.Commands
{
	public class ReturnOrderCommand : IRequest<ReservationResponse>
	{
		public long OrderId { get; set; }

	}
}
