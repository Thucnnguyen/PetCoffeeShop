using MediatR;
using PetCoffee.Application.Features.Reservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Reservation.Commands
{
    public class UpdateProductOfBookingCommand : IRequest<bool>
    {
        [JsonIgnore]
        public long OrderId { get; set; }

        public long ProductId { get; set; } 
        public int Quantity { get; set; }
    
    }
}
