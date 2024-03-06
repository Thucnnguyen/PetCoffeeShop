using MediatR;
using PetCoffee.Application.Features.Items.Models;
using PetCoffee.Application.Features.Memory.Models;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Items.Queries
{
    public class GetItemCurrentAccountIdQuery : IRequest<IList<ItemResponse>>
    {
        public long WalletId { get; set; }
    }
}
