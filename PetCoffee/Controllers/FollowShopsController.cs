
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.FollowShop.Commands;

namespace PetCoffee.API.Controllers
{
	[Route("api/followshops")]
	[ApiController]
	public class FollowShopsController : ApiControllerBase
	{
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
