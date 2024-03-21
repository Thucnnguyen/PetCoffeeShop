using MediatR;
using PetCoffee.Application.Features.Product.Models;

namespace PetCoffee.Application.Features.Product.Commands
{


    public class CreateProductCommand : IRequest<ProductResponse>
    {

        public long PetCoffeeShopId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
