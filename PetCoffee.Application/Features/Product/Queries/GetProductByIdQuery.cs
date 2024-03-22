using MediatR;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Product.Queries
{
   
    public class GetProductByIdQuery : IRequest<ProductResponse>
    {
        public long Id { get; set; }
    }

}
