using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Comment.Commands;
using PetCoffee.Application.Features.Comment.Models;
using PetCoffee.Application.Features.Comment.Queries;
using PetCoffee.Domain.Entities;

namespace PetCoffee.API.Controllers
{
	[Route("api/v1")]
	[ApiController]
	public class CommentController : ApiControllerBase
	{
		[HttpGet("comments/{PostId}")]
		[Authorize]
		public async Task<ActionResult<PaginationResponse<Comment, CommentResponse>>> GetCommentById([FromRoute] GetCommentByPostIdQuery request)
		{
			var response = await Mediator.Send(request);
			return response;
		}
		[HttpGet("comments/{CommentId}/subcomments")]
		[Authorize]
		public async Task<ActionResult<PaginationResponse<Comment, CommentResponse>>> GetsubCommentByCommentId([FromRoute] GetSubCommentByCommentIdQuery request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

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
		[HttpDelete("comments/{CommentId}")]
		[Authorize]
		public async Task<ActionResult<bool>> DeleteComment([FromRoute] DeleteCommentCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}
	}
}
