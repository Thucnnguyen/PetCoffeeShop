using MediatR;
using PetCoffee.Application.Features.Reservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Reservation.Queries
{
    public class GetReservationByAreaIdQuery : IRequest<IList<ReservationResponse>>
    {
        public long AreaId { get; set; }
    }
}
