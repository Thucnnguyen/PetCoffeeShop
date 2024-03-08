using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Application.Features.Events.Queries;
using PetCoffee.Application.Features.Items.Commands;
using PetCoffee.Application.Features.Items.Models;
using PetCoffee.Application.Features.Items.Queries;
using PetCoffee.Application.Features.Payments.Models;
using PetCoffee.Application.Features.Pet.Commands;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Pet.Queries;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Transactions.Commands;
using PetCoffee.Application.Features.Vaccination.Models;
using PetCoffee.Application.Features.Vaccination.Queries;
using PetCoffee.Domain.Entities;

namespace PetCoffee.API.Controllers
{
    [Route("/api/v1")]
    [ApiController]
    public class ItemController : ApiControllerBase
    {
        [HttpPost("items")]
        [Authorize]
        public async Task<ActionResult<ItemResponse>> CreateItem([FromForm] CreateItemCommand request)
        {
            var response = await Mediator.Send(request);
            return response;
        }

		[HttpPost("items/donation")]
		[Authorize]
		public async Task<ActionResult<PaymentResponse>> DonatePet([FromBody] DonationForPetCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		// get all item 
		[HttpGet("items")]
        [Authorize]
        public async Task<ActionResult<PaginationResponse<Item, ItemResponse>>> GetAllItems([FromQuery] GetAllItemQuery request)
        {
            var response = await Mediator.Send(request);
            return response;
        }

		[HttpGet("account/items")]
		[Authorize(Roles = "Customer")]
		public async Task<ActionResult<List<ItemWalletResponse>>> GetAllItemsInCurrentWallet([FromQuery] GetAllItemInWalletQuery request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		// get item by id 
		[HttpGet("item/{ItemId}")]
        [Authorize]
        public async Task<ActionResult<ItemResponse>> GetItemById([FromRoute] GetItemByIdQuery request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

    }

}
