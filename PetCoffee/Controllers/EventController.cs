using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Features.Events.Commands;
using PetCoffee.Application.Features.Events.Handlers;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Application.Features.Events.Queries;

namespace PetCoffee.API.Controllers
{
	[Route("api/v1")]
	[ApiController]
	public class EventController : ApiControllerBase
	{
		[HttpPost("events")]
		[Authorize]
		public async Task<ActionResult<EventResponse>> CreateEventForShop([FromForm] CreateEventCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpPost("events/fields")]
		[Authorize]
		public async Task<ActionResult<List<FieldEventResponseForEventResponse>>> Createfields([FromBody] CreateEventFieldCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpGet("events/{EventId}")]
		[Authorize]
		public async Task<ActionResult<EventResponse>> GetEvent([FromRoute] GetEventByIdQuery request)
		{
			var response = await Mediator.Send(request);
			return response;
		}
	}
}
