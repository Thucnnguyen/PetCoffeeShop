

using PetCoffee.Application.Features.Post.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Notifications.Models;

public class NotificationResponse
{
	public long Id { get; set; }

	public string Title { get; set; }
	public string Content { get; set; }
	public string Data { get; set; }
	public EntityType EntityType { get; set; }
	public string? ReferenceId { get; set; }
	public DateTimeOffset? ReadAt { get; set; }
	public bool IsRead { get; set; }
	public NotificationLevel Level { get; set; } = NotificationLevel.Information;

	public long AccountId { get; set; }
	public AccountForPostModel ToAccount { get; set; }
}
