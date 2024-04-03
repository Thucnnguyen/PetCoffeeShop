
using MediatR;
using PetCoffee.Application.Features.Wallets.Models;

namespace PetCoffee.Application.Features.Wallets.Queries;

public class GetOutcomeQuery : IRequest<IncomeForShopResponse>
{
	public int Months { get; set; }
}
