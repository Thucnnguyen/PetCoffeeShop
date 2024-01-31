using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.Auth.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetCoffee.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController :ApiControllerBase
	{
		// GET: api/<ValuesController>
		[HttpPost("login")]
		public async Task<ActionResult<AccessTokenResponse>> LoginUsernameAndPassword([FromBody] LoginUsernamePassCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		// GET api/<ValuesController>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<ValuesController>
		[HttpPost("Resgister")]
		public async Task<ActionResult<AccessTokenResponse>> ResgisterCustommer([FromBody] CustomerRegisterCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		
	}
}
