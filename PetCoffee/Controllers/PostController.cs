using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Completions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.Auth.Handlers;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Features.Post.Command;
using PetCoffee.Application.Features.Post.Commands;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Post.Queries;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetCoffee.API.Controllers
{
	[Route("/api/v1")]
	[ApiController]
	public class PostController : ApiControllerBase
	{
		// GET: api/<PostController>
		[HttpGet("/posts")]
		//[Authorize]
        public async Task<ActionResult<PaginationResponse<Post, PostResponse>>> GetAllPost(
            [FromQuery] GetAllPostQuery request)
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
            //await Mediator.Send(command);
            //return new StatusResponse(true);
        }



    }
}
