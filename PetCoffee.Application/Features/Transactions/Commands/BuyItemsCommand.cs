
using MediatR;

namespace PetCoffee.Application.Features.Transactions.Commands;

public class BuyItemsCommand : IRequest<bool>
{
    public IList<BuyItem> Items { get; set; }
}

public class BuyItem
{
    public long ItemId { get; set; }
    public int Quantity { get; set; }
}