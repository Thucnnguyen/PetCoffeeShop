using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.Tables.Models;
using PetCoffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Tables.Queries
{
    public class GetTableByAreaIdQuery : PaginationRequest<Table>, IRequest<PaginationResponse<Table, TableResponse>>
    {

        public long ShopId { get; set; }
        public long AreaId { get; set; }

        public override Expression<Func<Table, bool>> GetExpressions()
        {
            throw new NotImplementedException();
        }
    }
}
