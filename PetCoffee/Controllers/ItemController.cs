using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Items.Commands;
using PetCoffee.Application.Features.Items.Models;
using PetCoffee.Application.Features.Pet.Commands;
using PetCoffee.Application.Features.Pet.Models;

namespace PetCoffee.API.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemController : ApiControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ItemResponse>> CreateItem([FromForm] CreateItemCommand request)
        {
            var response = await Mediator.Send(request);
            return response;
        }

        
    }
    
}
