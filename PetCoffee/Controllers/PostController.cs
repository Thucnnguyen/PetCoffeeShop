﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Completions;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.Auth.Handlers;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Features.Post.Command;
using PetCoffee.Application.Features.Post.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetCoffee.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PostController : ApiControllerBase
	{
		// GET: api/<PostController>
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/<PostController>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<PostController>
		[HttpPost]
		[Authorize]
		public async Task<ActionResult<PostResponse>> Post([FromForm] CreatePostCommand request )
		{
			var response = await Mediator.Send(request);
			return response;
		}

		// PUT api/<PostController>/5
		[HttpPost("file")]
		public async Task<ActionResult<string>> PostFile([FromForm] FileCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		// DELETE api/<PostController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
