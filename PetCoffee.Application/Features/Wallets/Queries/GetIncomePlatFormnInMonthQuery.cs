
using MediatR;
using PetCoffee.Application.Features.Wallets.Models;

namespace PetCoffee.Application.Features.Wallets.Queries;

public class GetIncomePlatFormnInMonthQuery : IRequest<GetTransactionAmountResponse>
{
	public DateTimeOffset From { get; set; }
	public DateTimeOffset To { get; set; }
}
