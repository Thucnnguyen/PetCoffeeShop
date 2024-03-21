

using PetCoffee.Application.Service.Notifications;
using PetCoffee.Application.Service.Notifications.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Infrastructure.Services.Notifications;

public class NotificationAdapter : INotificationAdapter
{
    public Task<FirebaseNotification> ToFirebaseNotification(Notification notification, string deviceToken, DeviceType? deviceType = null)
    {
        var firebaseNotification = new FirebaseNotification()
        {
            Message = new()
            {
                Token = deviceToken,
                Notification = new()
                {
                    Title = notification.Title,
                    Body = notification.Content
                },
                Data = new
                {
                    Type = notification.Type.ToString(),
                    EntityType = notification.EntityType.ToString(),
                    ReferenceId = notification.ReferenceId
                },
            }
        };
        return Task.FromResult(firebaseNotification);
    }

    public async Task<WebNotification> ToWebNotification(Notification notification, string connectionId)
    {
        var webNotification = new WebNotification()
        {
            Id = notification.Id,
            AccountId = notification.AccountId,
            Type = notification.Type,
            EntityType = notification.EntityType,
            ReferenceId = notification.ReferenceId,
            Content = notification.Content,
            Data = notification.Data,
            ReadAt = notification.ReadAt,
            CreatedAt = notification.CreatedAt,
            Level = notification.Level,
            Title = notification.Title
        };

        return await Task.FromResult(webNotification);
    }
}
