﻿
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Service.Notifications;

public interface INotificationProvider
{
	void Attach(NotificationType type, INotificationService notificationService);

	void Attach(ICollection<NotificationType> types, INotificationService notificationService);

	void Attach(NotificationType type, ICollection<INotificationService> notificationServices);

	void Detach(NotificationType type, INotificationService notificationService);

	Task NotifyAsync(Notification notification);
}
