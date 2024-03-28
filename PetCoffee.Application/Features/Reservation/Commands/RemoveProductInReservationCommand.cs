using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Reservation.Commands
{
	public class RemoveProductInReservationCommand : IRequest<bool>
	{
		public long ReservationId { get; set; }
		public long ProductId { get; set; }
	}
}
