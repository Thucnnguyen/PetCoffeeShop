using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Features.Auth.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetCoffee.API.Controllers
{
	[Route("api/v1/auth")]
	[ApiController]
	public class AuthController : ApiControllerBase
	{
		[HttpPost("login")]
		public async Task<ActionResult<AccessTokenResponse>> LoginUsernameAndPassword([FromBody] LoginUsernamePassCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpGet("resend/OTP")]
		[Authorize]
		public async Task<ActionResult<bool>> Resend([FromQuery] ResendOTPQuery request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpGet("checkemail/{Email}")]
		[Authorize]
		public async Task<ActionResult<bool>> CheckEmail([FromRoute] CheckEmailExistQuery request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpGet("forgotpassword/resend/OTP")]
		[Authorize]
		public async Task<ActionResult<bool>> ResendOTPForForgotPassword([FromQuery] ResendOTPQuery request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpPut("forgotpassword/password")]
		public async Task<ActionResult<bool>> ChangePasswordForForgotPassword([FromBody] ChangePasswordForForgotCommand request)
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
		[HttpPost("register")]
		public async Task<ActionResult<AccessTokenResponse>> ResgisterCustommer([FromForm] CustomerRegisterCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpPost("verify/OTP")]
		[Authorize]
		public async Task<ActionResult<bool>> VerifyCustommer([FromBody] VerifyAccountCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpPut("")]
		[Authorize]
		public async Task<ActionResult<AccountResponse>> UpdateAccount([FromForm] UpdateAccountCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpPut("password")]
		[Authorize]
		public async Task<ActionResult<AccountResponse>> UpdatePassword([FromBody] UpdatePasswordCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

	}
}
