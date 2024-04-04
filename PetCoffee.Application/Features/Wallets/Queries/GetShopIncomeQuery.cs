

using MediatR;
using PetCoffee.Application.Features.Wallets.Models;

namespace PetCoffee.Application.Features.Wallets.Queries;

public class GetShopIncomeQuery : IRequest<IncomeResponse>
{
    public long ShopId { get; set; }
    public int Months { get; set; }
}
