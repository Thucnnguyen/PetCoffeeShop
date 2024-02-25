
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.FollowShop.Commands;
using PetCoffee.Application.Features.FollowShop.Queries;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Domain.Entities;

namespace PetCoffee.API.Controllers
{
	[Route("api/followshops")]
	[ApiController]
	public class FollowShopsController : ApiControllerBase
	{
		[HttpGet("petcoffeeshops/followshops")]
		[Authorize]
		public async Task<ActionResult<PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>>> GetFollowShop([FromQuery] GetFollowShopForCurrentUserQuery request)
		{
			var response = await Mediator.Send(request);
			return response;
		}
		[HttpPost("{PetCoffeeShopId}")]
		[Authorize]
		public async Task<ActionResult<bool>> CreateFollowShop([FromRoute] CreateFollowShopCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}
		[HttpDelete("{PetCoffeeShopId}")]
		[Authorize]
		public async Task<ActionResult<bool>> UnFollowShop([FromRoute] DeleteFollowShopCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}
	}
}
