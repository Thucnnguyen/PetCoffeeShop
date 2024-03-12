using PetCoffee.Application.Features.Transaction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Reservation.Models
{
    public class ReservationDetailResponse: ReservationResponse
    {
        public IList<TransactionResponse> Transactions { get; set; } = new List<TransactionResponse>();
    }
}
