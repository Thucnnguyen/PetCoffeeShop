using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Payments.Commands;
using PetCoffee.Application.Features.Payments.Models;

namespace PetCoffee.API.Controllers
{
	[Route("api/v1")]
	[ApiController]
	public class DepositController : ApiControllerBase
	{
		[HttpPost("deposits")]
		public async Task<ActionResult<PaymentResponse>> CreateDepositTransaction ([FromBody] CreatePaymentCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}
		[HttpPost("deposits/callback")]
		public async Task<ActionResult<PaymentResponse>> CallbackZaloPay([FromBody] CallbackZaloPayCommand request)
		{
			await Mediator.Send(request);
			return Ok();
		}
	}
}
