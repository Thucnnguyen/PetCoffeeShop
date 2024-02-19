using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Pet.Queries;
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
        [HttpGet("petcoffeeshops")]
        public async Task<ActionResult<PaginationResponse<PetCoffeeShop, PetCoffeeShopResponse>>> GetAllPetCfShop(
			[FromQuery] GetAllPetCfShopQuery request)
        {
            return await Mediator.Send(request);
        }
		[HttpGet("petcoffeeshops/{Id}")]
		public async Task<ActionResult< PetCoffeeShopResponse>> GetPetCfShopById(
			[FromQuery] GetPetCoffeeShopByIdQuery request)
		{
			return await Mediator.Send(request);
		}

		[HttpPost("petcoffeeshops")]
        [Authorize(Roles ="Customer")]
		public async Task<ActionResult< PetCoffeeShopResponse>> AddPetCfShop(
        [FromForm] CreatePetCfShopCommand request)
		{
			return await Mediator.Send(request);
		}

		[HttpPut("petcoffeeshops")]
		[Authorize]
		public async Task<ActionResult<PetCoffeeShopResponse>> UpdatePetCfShop(
		[FromForm] UpdateCoffeeShopCommand request)
		{
			return await Mediator.Send(request);
		}
		[HttpGet("accounts/petcoffeeshops")]
		[Authorize]
		public async Task<ActionResult<PetCoffeeShopResponse>> GetCoffeeShopResponse(
		[FromForm] GetPetCfShopForCurrentAccountQuery request)
		{
			return await Mediator.Send(request);
		}

		[HttpGet("petcoffeeshops/taxcode/{TaxCode}")]
		public async Task<ActionResult<TaxCodeResponse>> CheckTaxCode(
		[FromRoute] CheckTaxCodeQuery request)
		{
			return await Mediator.Send(request);
		}
	}
}
