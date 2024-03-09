using MediatR;
using PetCoffee.Application.Features.Payments.Models;

namespace PetCoffee.Application.Features.Payments.Queries;

public class GetTransactionByIdQuery : IRequest<PaymentResponse>
{
	public long TransactionId { get; set; }
}
