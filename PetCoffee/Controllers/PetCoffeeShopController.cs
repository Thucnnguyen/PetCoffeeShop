using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Commands;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Domain.Entities;

namespace PetCoffee.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("/api/v1")]
    [ApiController]
    public class PetCoffeeShopController : ApiControllerBase
    {
        // get all paging
        [HttpGet("Petcoffeeshops")]
        public async Task<ActionResult<PaginationResponse<PetCoffeeShop, PetCoffeeShopResponse>>> GetAllPetCfShop(
			[FromQuery] GetAllPetCfShopQuery request)
        {
            return await Mediator.Send(request);
        }
		[HttpGet("Petcoffeeshops/{Id}")]
		public async Task<ActionResult< PetCoffeeShopResponse>> GetPetCfShopById(
			[FromQuery] GetPetCoffeeShopByIdQuery request)
		{
			return await Mediator.Send(request);
		}

		[HttpPost("Petcoffeeshops")]
        [Authorize(Roles ="Customer")]
		public async Task<ActionResult< PetCoffeeShopResponse>> AddPetCfShop(
        [FromForm] CreatePetCfShopCommand request)
		{
			return await Mediator.Send(request);
		}

		[HttpPut("Petcoffeeshops")]
		[Authorize]
		public async Task<ActionResult<PetCoffeeShopResponse>> UpdatePetCfShop(
		[FromForm] UpdateCoffeeShopCommand request)
		{
			return await Mediator.Send(request);
		}
		[HttpGet("Accounts/Petcoffeeshops")]
		[Authorize]
		public async Task<ActionResult<PetCoffeeShopResponse>> GetCoffeeShopResponse(
		[FromForm] GetPetCfShopForCurrentAccount request)
		{
			return await Mediator.Send(request);
		}
	}
}
