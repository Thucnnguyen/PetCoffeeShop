using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Domain.Entities;

namespace PetCoffee.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("/api/v1/petcoffeeshop")]
    [ApiController]
    public class PetCoffeeShopController : ApiControllerBase
    {
        // get all paging
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<PetCoffeeShop, PetCoffeeShopResponse>>> GetAllPetCfShop(
       [FromQuery] GetAllPetCfShopQuery request)
        {
            return await Mediator.Send(request);
        }

    }
}
