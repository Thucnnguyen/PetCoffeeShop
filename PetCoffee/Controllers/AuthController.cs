using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Features.Auth.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetCoffee.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ApiControllerBase
	{
		[HttpPost("login")]
		public async Task<ActionResult<AccessTokenResponse>> LoginUsernameAndPassword([FromBody] LoginUsernamePassCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpGet("Resend/OTP")]
		[Authorize]
		public async Task<ActionResult<bool>> Resend([FromQuery] ResendOTPQuery request)
		{
			var response = await Mediator.Send(request);
			return response;
		}
		[HttpGet("")]
		[Authorize]
		public async Task<ActionResult<AccountResponse>> GetCurrentAccount([FromQuery] GetCurrentAccountInfomationQuery request)
		{
			return await Mediator.Send(request);
		}
		[HttpGet("{Id}")]
		[Authorize]
		public async Task<ActionResult<AccountResponse>> GetAccount([FromRoute] GetAccountInformationByIdQuery request)
		{
			return await Mediator.Send(request);
		}

		// POST api/<ValuesController>
		[HttpPost("Resgister")]
		public async Task<ActionResult<AccessTokenResponse>> ResgisterCustommer([FromForm] CustomerRegisterCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpPost("Verify/OTP")]
		[Authorize]
		public async Task<ActionResult<bool>> VerifyCustommer([FromBody] VerifyAccountCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		
	}
}
