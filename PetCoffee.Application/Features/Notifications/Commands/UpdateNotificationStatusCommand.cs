

using MediatR;
using PetCoffee.Application.Features.Notifications.Models;

namespace PetCoffee.Application.Features.Notifications.Commands;

public class UpdateNotificationStatusCommand : IRequest<NotificationResponse>
{
	public long Id { get; set; }

	public bool? IsRead { get; set; }
}
