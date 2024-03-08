

using MediatR;
using PetCoffee.Application.Features.Payments.Models;

namespace PetCoffee.Application.Features.Transactions.Commands;

public class DonationForPetCommand : IRequest<PaymentResponse>
{
	public long PetId { get; set; }	
	public List<BuyItem> DonateItems { get; set; }
}
