using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Wallets.Models;
using PetCoffee.Application.Features.Wallets.Queries;

namespace PetCoffee.API.Controllers;

[Route("api/v1")]
[ApiController]
public class WalletController : ApiControllerBase
{
	[HttpGet("account/wallet")]
	[Authorize]
	public async Task<ActionResult<WalletResponse>> GetAreaById( [FromQuery]GetWalletForCurrentAccountQuery request)
	{
		var response = await Mediator.Send(request);
		return Ok(response);
	}
}
