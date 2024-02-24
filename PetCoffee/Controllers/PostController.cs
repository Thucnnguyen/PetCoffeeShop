using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Post.Command;
using PetCoffee.Application.Features.Post.Commands;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Post.Queries;
using PetCoffee.Domain.Entities;

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
		//[Authorize]
        public async Task<ActionResult<PaginationResponse<Post, PostResponse>>> GetAllPost(
            [FromQuery] GetAllPostQuery request)
        {
            return await Mediator.Send(request);
        }

        [HttpGet("/posts/news-feed")]
        [Authorize]
        public async Task<ActionResult<PaginationResponse<Post, PostResponse>>> GetAllPostNewsFeed(
        [FromQuery] GetPostsNewsFeedQuery request)
        {
            return await Mediator.Send(request);
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

        [HttpPut("{id:long}/posts/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> UpdatePostStatus([FromRoute] long id, [FromBody] UpdatePostStatusCommand request)
        {
            request.Id = id;
            var response = await Mediator.Send(request);
            return response;
        }

        [HttpGet("/posts/{Id:long}")]
        [Authorize]
        public async Task<ActionResult<PostResponse>> GetPost([FromRoute] GetPostByIdQuery request)
        {

            return await Mediator.Send(request);
        }
    }
}
