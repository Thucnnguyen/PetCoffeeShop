using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Events.Commands;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Application.Features.Events.Queries;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.SubmitttingEvents.Commands;
using PetCoffee.Application.Features.SubmitttingEvents.Models;
using PetCoffee.Domain.Entities;

namespace PetCoffee.API.Controllers
{
	[Route("api/v1")]
	[ApiController]
	public class EventController : ApiControllerBase
	{
		[HttpPost("events")]
		[Authorize(Roles = "Manager,Staff")]
		public async Task<ActionResult<EventResponse>> CreateEventForShop([FromForm] CreateEventCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpPost("events/fields")]
		[Authorize(Roles = "Manager,Staff")]
		public async Task<ActionResult<List<FieldEventResponseForEventResponse>>> Createfields([FromBody] CreateEventFieldCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpPost("events/joinevents")]
		[Authorize]
		public async Task<ActionResult<SubmittingEventResponse>> JoinEvent([FromBody] CreateSubmittingEventCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpPut("events")]
		[Authorize(Roles = "Manager,Staff")]
		public async Task<ActionResult<EventResponse>> updateEvent([FromForm] UpdateEventCommand request)
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
		[HttpGet("events/{EventId}/submitting-event-for-customer")]
		[Authorize]
		public async Task<ActionResult<EventResponse>> GetsubmitingEventForCustomer([FromRoute] GetSubmitEventByEvenIdFormForCustomerQuery request)
		{
			var response = await Mediator.Send(request);
			return response;
		}
		[HttpGet("events/joinevents")]
		[Authorize]
		public async Task<ActionResult<PaginationResponse<SubmittingEvent, EventForCardResponse>>> GetJoinEvent([FromQuery] GetJoinEventForCustomerQuery request)
		{
			var response = await Mediator.Send(request);
			return response;
		}
		[HttpGet("events")]
		[Authorize]
		public async Task<ActionResult<PaginationResponse<Event, EventResponse>>> GetEventForCustomer([FromQuery] GetEventsForCustomerQuery request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpDelete("events/{EventId}")]
		[Authorize]
		public async Task<ActionResult<bool>> DeleteEvent([FromRoute] DeleteEventCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpDelete("events-fields/{EventFieldId}")]
		[Authorize]
		public async Task<ActionResult<bool>> DeleteEvent([FromRoute] DeleteEventFieldCommand request)
		{
			var response = await Mediator.Send(request);
			return response;
		}

		[HttpGet("petcoffeeshops/{ShopId}/events")]
		[Authorize]
		public async Task<ActionResult<List<EventForCardResponse>>> GetEventByShopId([FromRoute] GetEventsByShopIdQuery request)
		{
			var response = await Mediator.Send(request);
			return response;
		}
	}
}
