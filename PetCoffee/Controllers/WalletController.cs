using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Items.Models;
using PetCoffee.Application.Features.Items.Queries;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Post.Queries;

namespace PetCoffee.API.Controllers
{
    [Route("/api/v1")]
    [ApiController]
    public class WalletController : ApiControllerBase
    {
        [HttpGet("currentAccounts/wallet/{WalletId}/items")]
        [Authorize]
        public async Task<ActionResult<IList<ItemResponse>>> GetItemsByCurrentAccount([FromRoute] GetItemCurrentAccountIdQuery request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}
