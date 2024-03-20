using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Comment.Commands;
using PetCoffee.Application.Features.Comment.Models;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Pet.Queries;
using PetCoffee.Application.Features.Product.Commands;
using PetCoffee.Application.Features.Product.Models;

namespace PetCoffee.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ProductController : ApiControllerBase
    {

        [HttpPost("product")]
        [Authorize]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromForm] CreateProductCommand request)
        {
            var response = await Mediator.Send(request);
            return response;
        }

        // 
    }
}
