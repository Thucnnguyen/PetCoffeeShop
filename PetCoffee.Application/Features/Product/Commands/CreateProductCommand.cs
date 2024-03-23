using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Product.Models;

namespace PetCoffee.Application.Features.Product.Commands
{


	public class CreateProductCommand : IRequest<ProductResponse>
	{

		public long PetCoffeeShopId { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
        public IList<IFormFile> Image { get; set; }
    }
}
