using MediatR;
using PetCoffee.Application.Features.Notifications.Models;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Notifications.Queries;

public class GetUnreadNotificationQuery : IRequest<UnreadNotificationCountResponse>
{
	public NotificationType? Type { get; set; }

	public EntityType? EntityType { get; set; }

	public DateTimeOffset? From { get; set; }

	public DateTimeOffset? To { get; set; }
}
