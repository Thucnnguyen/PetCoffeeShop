using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Items.Models;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Items.Queries
{
    public class GetAllItemQuery : PaginationRequest<Item>, IRequest<PaginationResponse<Item, ItemResponse>>
    {

        public override Expression<Func<Item, bool>> GetExpressions()
        {
            throw new NotImplementedException();
        }
    }
}
