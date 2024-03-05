using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Notifications.Models;
using PetCoffee.Application.Features.Notifications.Queries;
using PetCoffee.Domain.Entities;

namespace PetCoffee.API.Controllers;

[Route("api/v1")]
[ApiController]
public class NotificationController : ApiControllerBase
{
	[HttpGet("notifications")]
	[Authorize]

	public async Task<ActionResult<PaginationResponse<Notification, NotificationResponse>>> GetNotificaiton([FromQuery] GetAllNotificationQuery request)
	{
		var response = await Mediator.Send(request);
		return Ok(response);
	}

	[HttpGet("notifications/unread")]
	[Authorize]
	public async Task<ActionResult<UnreadNotificationCountResponse>> CountNotificaitonUnread([FromQuery] GetUnreadNotificationQuery request)
	{
		var response = await Mediator.Send(request);
		return Ok(response);
	}
}
