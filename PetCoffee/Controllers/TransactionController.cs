using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Transactions.Commands;

namespace PetCoffee.API.Controllers;

[Route("api/v1")]
[ApiController]
public class TransactionController : ApiControllerBase
{
	[HttpPost("transaction/items")]
	[Authorize]
	public async Task<ActionResult<bool>> BuyItems([FromBody] BuyItemsCommand request)
	{
		var response = await Mediator.Send(request);
		return response;
	}
}
