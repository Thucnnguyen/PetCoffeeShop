
using MediatR;
using PetCoffee.Application.Features.Wallets.Models;

namespace PetCoffee.Application.Features.Wallets.Queries;

public class GetIncomeQuery : IRequest<IncomeForShopResponse>
{
	public int Months { get; set; }
}
