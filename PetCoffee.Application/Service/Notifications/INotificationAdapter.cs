

using PetCoffee.Application.Service.Notifications.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Service.Notifications;

public interface INotificationAdapter
{
    public Task<WebNotification> ToWebNotification(Notification notification, string connectionId);

    public Task<FirebaseNotification> ToFirebaseNotification(Notification notification, string deviceToken, DeviceType? deviceType = null);
}
