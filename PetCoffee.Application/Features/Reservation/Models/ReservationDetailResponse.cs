using PetCoffee.Application.Features.Transactions.Models;

namespace PetCoffee.Application.Features.Reservation.Models
{
    public class ReservationDetailResponse : ReservationResponse
    {
        public IList<TransactionResponse> Transactions { get; set; } = new List<TransactionResponse>();
    }
}
