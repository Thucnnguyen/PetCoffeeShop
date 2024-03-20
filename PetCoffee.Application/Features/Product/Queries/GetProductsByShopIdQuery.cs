using MediatR;
using PetCoffee.Application.Common.Models.Request;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Product.Queries
{
  

    public class GetProductsByShopIdQuery : PaginationRequest<Domain.Entities.Product>, IRequest<PaginationResponse<Domain.Entities.Product, ProductResponse>>
    {
        public long ShopId { get; set; }

        public override Expression<Func<Domain.Entities.Product, bool>> GetExpressions()
        {
            throw new NotImplementedException();
        }
    }
}
