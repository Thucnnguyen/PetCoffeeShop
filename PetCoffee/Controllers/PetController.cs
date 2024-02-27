using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Pet.Commands;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Pet.Queries;

namespace PetCoffee.API.Controllers;

[Route("api/v1")]
[ApiController]
public class PetController : ApiControllerBase
{
	[HttpPost("Pets")]
	[Authorize]
	public async Task<ActionResult<PetResponse>> CreatePet([FromForm] CreatePetCommand request)
	{
		var response = await Mediator.Send(request);
		return response;
	}
	[HttpGet("PetCoffeeShops/{ShopId}/Pets")]
	[Authorize]
	public async Task<ActionResult<IList<PetResponse>>> GetPetByShopId([FromRoute] GetPetsByShopIdQuery request)
	{
		var response = await Mediator.Send(request);
		return Ok(response);
	}
	[HttpGet("Pets/{Id}")]
	[Authorize]
	public async Task<ActionResult<IList<PetResponse>>> GetPetById([FromRoute] GetPetByIdQuery request)
	{
		var response = await Mediator.Send(request);
		return Ok(response);
	}



}
