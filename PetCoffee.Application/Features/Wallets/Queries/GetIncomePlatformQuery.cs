
using MediatR;
using PetCoffee.Application.Features.Wallets.Models;

namespace PetCoffee.Application.Features.Wallets.Queries;

public class GetIncomePlatformQuery : IRequest<IncomeResponse>
{
	public int Months { get; set; }
}
