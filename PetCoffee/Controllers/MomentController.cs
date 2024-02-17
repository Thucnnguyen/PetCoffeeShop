﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Memory.Commands;
using PetCoffee.Application.Features.Memory.Models;
using PetCoffee.Application.Features.Moment.Queries;

namespace PetCoffee.API.Controllers;

[Route("/api/v1")]
[ApiController]
public class MomentController : ApiControllerBase
{

	[HttpPost("moments")]
	[Authorize]
	public async Task<ActionResult<MomentResponse>> CreateMoment([FromForm] CreateMomentCommand request)
	{
		var response = await Mediator.Send(request);
		return response;
	}

	[HttpGet("pets/{Id}/moments")]
	[Authorize]
	public async Task<ActionResult<IList<MomentResponse>>> GetMomentByPetId([FromRoute] GetMomentByPetIdQuery request)
	{
		var response = await Mediator.Send(request);
		return Ok(response);
	}
}
