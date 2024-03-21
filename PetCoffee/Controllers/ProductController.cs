using Microsoft.AspNetCore.Mvc;

namespace PetCoffee.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ProductController : ApiControllerBase
    {

        //[HttpPost("product")]
        //[Authorize]
        //public async Task<ActionResult<ProductResponse>> CreateProduct([FromForm] CreateProductCommand request)
        //{
        //    var response = await Mediator.Send(request);
        //    return response;
        //}

        //// 
        //[HttpGet("petCoffeeShops/products")]
        //[Authorize]
        //public async Task<ActionResult<PaginationResponse<Domain.Entities.Product, ProductResponse>>> GetProductsByShopId([FromQuery] GetProductsByShopIdQuery request)
        //{
        //    var response = await Mediator.Send(request);
        //    return Ok(response);
        //}

    }
}
