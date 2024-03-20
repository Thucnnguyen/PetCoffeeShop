using MediatR;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Product.Commands
{

    
    public class CreateProductCommand : IRequest<ProductResponse>
    {

        public long PetCoffeeShopId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
