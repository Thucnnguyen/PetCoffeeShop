using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.PostCategory.Commands;
using PetCoffee.Application.Features.PostCategory.Models;
using PetCoffee.Application.Features.PostCategory.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetCoffee.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PostCategoryController : ApiControllerBase
	{

		// GET api/<PostCategoryController>/5
		[HttpGet]
		public async Task<ActionResult<IList<PostCategoryResponse>>> Get([FromQuery]GetAllCategoriesQuery request)
		{
			return Ok(await Mediator.Send(request));
		}

		// POST api/<PostCategoryController>
		[HttpPost("")]

		public async Task<ActionResult<PostCategoryResponse>> Post([FromBody] CreatePostCategoryCommand request)
		{
			return await Mediator.Send(request);
		}

		// PUT api/<PostCategoryController>/5
		[HttpPut()]
		public async Task<ActionResult<bool>> Put([FromBody] UpdatePostCategoryCommand request)
		{
			return Ok(await Mediator.Send(request));
		}

		// DELETE api/<PostCategoryController>/5
		[HttpDelete()]
		public async Task<ActionResult<bool>> Delete([FromQuery] RemovePostCategoryCommand request)
		{
			return Ok(await Mediator.Send(request));
		}
	}
}
