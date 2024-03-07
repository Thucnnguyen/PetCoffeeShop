using MediatR;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Application.Features.Items.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Items.Queries
{
    public class GetItemByIdQuery : IRequest<ItemResponse>
    {
        public long ItemId { get; set; }
    }
}
