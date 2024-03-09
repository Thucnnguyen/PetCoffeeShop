using MediatR;
using PetCoffee.Application.Features.Reservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Reservation.Commands
{
    public class ReturnOrderCommand : IRequest<ReservationResponse>
    {
        public long OrderId { get; set; }

    }
}
