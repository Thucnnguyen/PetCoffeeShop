using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Features.Auth.Queries;
using PetCoffee.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetCoffee.API.Controllers
{
	[Route("api/v1/auths")]
	[ApiController]
	public class AuthController : ApiControllerBase
	{
		[HttpPost("login")]
		public async Task<ActionResult<AccessTokenResponse>> LoginEmailAndPassword([FromBody] LoginEmailPassCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpPost("login/firebase")]
		public async Task<ActionResult<AccessTokenResponse>> Loginfirebase([FromBody] VerifyFirebaseTokenCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpPost("pet-coffee-shops/staffs")]
		[Authorize(Roles = "Manager")]
		public async Task<ActionResult<bool>> CreateStaffAccount([FromBody] RegisterShopStaffAccountCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpGet("pet-coffee-shops/staffs")]
		[Authorize(Roles = "Manager")]
		public async Task<ActionResult<PaginationResponse<Account, AccountResponse>>> GetListStaffs([FromQuery] GetStaffAccountByShopIdQuery request)
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
		public async Task<ActionResult<bool>> CheckEmail([FromRoute] CheckEmailExistQuery request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpGet("forgotpassword/resend/OTP")]
		public async Task<ActionResult<bool>> ResendOTPForForgotPassword([FromQuery] SendOTPForForgotPasswordCommand request)
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

		[HttpGet("accounts")]
		[Authorize(Roles = "Admin,PlatforStaff")]
		public async Task<ActionResult<PaginationResponse<Account, AccountForRecord>>> GetAllAccount([FromQuery] GetAllAccountsQuery request)
		{
			return await Mediator.Send(request);
		}
		[HttpPut("accounts/status")]
		[Authorize(Roles = "Admin,PlatforStaff,Manager")]
		public async Task<ActionResult<bool>> updateAccountStatus([FromBody] UpdateAccountStatusCommand request)
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
		[HttpPost("register/platformstaffs")]
		public async Task<ActionResult<bool>> ResgisterPlatFormStaff ([FromBody] CreateAccountStaffPlaformCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}
		[HttpPost("verify/otp")]
		[Authorize]
		public async Task<ActionResult<bool>> VerifyCustommer([FromBody] VerifyAccountCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}
		[HttpPost("forgotpassword/verify/otp")]
		public async Task<ActionResult<bool>> VerifyForForgotPassword([FromBody] VerifiedOTPForForgotPasswordCommand request)
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
		[HttpPut("staffs/password")]
		[Authorize]
		public async Task<ActionResult<bool>> UpdateStaffPassword([FromBody] ChangeStaffPasswordCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}
	}
}
