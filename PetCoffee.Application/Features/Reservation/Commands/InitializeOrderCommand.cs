using MediatR;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Reservation.Commands
{
    public class InitializeOrderCommand : IRequest<ReservationResponse>
    {
        public long AreaId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Note { get; set; }

        public int TotalSeatBook { get; set; }
    }
}
