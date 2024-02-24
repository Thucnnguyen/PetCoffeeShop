using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Comment.Commands;
using PetCoffee.Application.Features.Comment.Models;

namespace PetCoffee.API.Controllers
{
	[Route("api/v1")]
	[ApiController]
	public class CommentController : ApiControllerBase
	{
		[HttpPost("comments")]
		[Authorize]
		public async Task<ActionResult<CommentResponse>> CreateComment([FromForm] CreateCommentCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}
		[HttpPut("comments")]
		[Authorize]
		public async Task<ActionResult<CommentResponse>> UpdateComment([FromForm] UpdateCommentCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}
	}
}
