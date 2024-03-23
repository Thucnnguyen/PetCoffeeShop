using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Pet.Commands;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Pet.Queries;
using PetCoffee.Application.Features.Product.Commands;
using PetCoffee.Application.Features.Product.Models;
using PetCoffee.Application.Features.Product.Queries;

namespace PetCoffee.API.Controllers
{
	[Route("api/v1")]
	[ApiController]
	public class ProductController : ApiControllerBase
	{

        [HttpPost("products")]
        [Authorize]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromForm] CreateProductCommand request)
        {
            var response = await Mediator.Send(request);
            return response;
        }

        [HttpGet("petCoffeeShops/products")]
        [Authorize]
        public async Task<ActionResult<PaginationResponse<Domain.Entities.Product, ProductResponse>>> GetProductsByShopId([FromQuery] GetProductsByShopIdQuery request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }


        [HttpGet("products/{Id}")]
        [Authorize]
        public async Task<ActionResult<ProductResponse>> GetProductById([FromRoute] GetProductByIdQuery request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("products/{Id}")]
        [Authorize]
        public async Task<ActionResult<bool>> UpdateProduct([FromRoute] DeleteProductCommand request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("products")]
        [Authorize]
        public async Task<ActionResult<ProductResponse>> UpdateProduct([FromForm] UpdateProductCommand request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

    }
}
