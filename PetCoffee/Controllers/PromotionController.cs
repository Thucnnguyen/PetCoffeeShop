using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Promotion.Commands;
using PetCoffee.Application.Features.Promotion.Models;
using PetCoffee.Application.Features.Promotion.Queries;

namespace PetCoffee.API.Controllers
{
	[Route("api/v1")]
	[ApiController]

	public class PromotionController : ApiControllerBase
	{
		[HttpPost("promotion")]
		[Authorize]
		public async Task<ActionResult<PromotionResponse>> CreatePromotionForShop([FromForm] CreatePromotionForShopCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpPut("promotion")]
		[Authorize]
		public async Task<ActionResult<bool>> UpdatePromotionForShop([FromBody] UpdatePromotionForShopCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpGet("petCoffeeShops/promotion")]
		[Authorize]
		public async Task<ActionResult<PaginationResponse<Domain.Entities.Promotion, PromotionResponse>>> GetPromotionByShopId([FromQuery] GetPromotionsByShopIdQuery request)
		{
			var response = await Mediator.Send(request);
			return Ok(response);
		}

		[HttpGet("promotion/{Id}")]
		[Authorize]
		public async Task<ActionResult<PromotionResponse>> GetPromotionById([FromRoute] GetPromotionByIdQuery request)
		{
			var response = await Mediator.Send(request);
			return Ok(response);
		}


		[HttpDelete("promotion/{Id}")]
		[Authorize]
		public async Task<ActionResult<bool>> DeletePromotion([FromRoute] DeletePromotionCommand request)
		{
			var response = await Mediator.Send(request);
			return Ok(response);
		}

	}
}
