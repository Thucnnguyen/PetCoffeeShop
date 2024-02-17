using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Completions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.Auth.Handlers;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Features.Post.Command;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Post.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetCoffee.API.Controllers
{
	[Route("/api/v1")]
	[ApiController]
	public class PostController : ApiControllerBase
	{
		// GET: api/<PostController>
		[HttpGet("/posts")]
		[Authorize]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/<PostController>/5
		[HttpGet("currentAccounts/posts")]
		[Authorize]
		public async Task<ActionResult<IList<PostResponse>>> GetPostCreateByCurrentAccount([FromQuery]GetPostCreatedByCurrentAccountIdQuery request)
		{
			var response = await Mediator.Send(request);
			return Ok(response);
		}

		// POST api/<PostController>
		[HttpPost("posts")]
		[Authorize]
		public async Task<ActionResult<PostResponse>> Post([FromForm] CreatePostCommand request )
		{
			var response = await Mediator.Send(request);
			return response;
		}
	}
}
