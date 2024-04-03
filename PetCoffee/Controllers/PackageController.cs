using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Packages.Commands;
using PetCoffee.Application.Features.Packages.Models;
using PetCoffee.Application.Features.Packages.Queries;

namespace PetCoffee.API.Controllers;

[Route("api/v1")]
[ApiController]
public class PackageController : ApiControllerBase
{
	[HttpGet("packages")]
	[Authorize]
	public async Task<ActionResult<List<PackageResponse>>> GetAllPackages([FromRoute] GetAllPackageQuery request)
	{
		var response = await Mediator.Send(request);
		return response;
	}

	[HttpGet("packages/{Id}")]
	[Authorize]
	public async Task<ActionResult<PackageResponse>> GetPackagesById([FromRoute] GetPackageByIdQuery request)
	{
		var response = await Mediator.Send(request);
		return response;
	}


	[HttpPost("packages")]
	[Authorize]
	public async Task<ActionResult<PackageResponse>> CreatePackage([FromBody] CreatePackageCommand request)
	{
		var response = await Mediator.Send(request);
		return response;
	}

	[HttpPost("shops/packages")]
	[Authorize]
	public async Task<ActionResult<bool>> BuyPackage([FromBody] BuyPackageCommand request)
	{
		var response = await Mediator.Send(request);
		return response;
	}

	[HttpPut("packages")]
	[Authorize]
	public async Task<ActionResult<PackageResponse>> CreatePackage([FromBody] UpdatePackageCommand request)
	{
		var response = await Mediator.Send(request);
		return response;
	}

	[HttpDelete("packages/{Id}")]
	[Authorize]
	public async Task<ActionResult<bool>> UpdatePackage([FromRoute] DeletePackageCommand request)
	{
		var response = await Mediator.Send(request);
		return response;
	}



}
