

using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Service.Notifications;

public interface INotificationService
{
	public Task NotifyAsync(Notification notification);
}
