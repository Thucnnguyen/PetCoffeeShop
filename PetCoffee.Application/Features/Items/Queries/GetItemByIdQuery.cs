using MediatR;
using PetCoffee.Application.Features.Items.Models;

namespace PetCoffee.Application.Features.Items.Queries
{
    public class GetItemByIdQuery : IRequest<ItemResponse>
    {
        public long ItemId { get; set; }
    }
}
