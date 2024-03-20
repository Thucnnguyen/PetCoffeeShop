using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.RatePets.Commands;
using PetCoffee.Application.Features.RatePets.Models;
using PetCoffee.Application.Features.RatePets.Queries;
using PetCoffee.Domain.Entities;

namespace PetCoffee.API.Controllers;

[Route("api/v1")]
[ApiController]
public class RatePetController : ApiControllerBase
{
	[HttpPost("rate-pets")]
	[Authorize]
	public async Task<ActionResult<RatePetResponse>> CreateRatePet([FromBody] CreatePetRateCommand request)
	{
		var response = await Mediator.Send(request);
		return response;
	}

	[HttpGet("pet/{PetId}/rate-pets")]
	[Authorize]
	public async Task<PaginationResponse<RatePet, RatePetResponse>> GetRatePets([FromRoute] GetPetRateQuery request)
	{
		var response = await Mediator.Send(request);
		return response;
	}
	[HttpGet("pet/{PetId}/rate-pets/random")]
	[Authorize]
	public async Task<RatePetResponse> GetRatePetRandom([FromRoute] GetRandomRatePetQuery request)
	{
		var response = await Mediator.Send(request);
		return response;
	}
}
