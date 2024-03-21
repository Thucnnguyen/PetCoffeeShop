

using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Service.Notifications;

public interface INotifier
{
    Task NotifyAsync(Notification notification, bool IsSaved);
}
