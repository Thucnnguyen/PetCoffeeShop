using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Items.Commands;
using PetCoffee.Application.Features.Items.Models;
using PetCoffee.Application.Features.Items.Queries;
using PetCoffee.Application.Features.Pet.Commands;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Vaccination.Models;
using PetCoffee.Application.Features.Vaccination.Queries;

namespace PetCoffee.API.Controllers
{
    [Route("/api/v1")]
    [ApiController]
    public class ItemController : ApiControllerBase
    {
        [HttpPost("itemss")]
        [Authorize]
        public async Task<ActionResult<ItemResponse>> CreateItem([FromForm] CreateItemCommand request)
        {
            var response = await Mediator.Send(request);
            return response;
        }

        // get all item 


        // get item by id 
        [HttpGet("item/{ItemId}")]
        [Authorize]
        public async Task<ActionResult<IList<ItemResponse>>> GetItemsById([FromRoute] GetItemByIdQuery request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

    }

}
