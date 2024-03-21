

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service.Notifications;
using PetCoffee.Application.Service.Notifications.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Infrastructure.Services.Notifications;

public class Notifier : INotifier
{
    private readonly INotificationProvider _provider;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<Notifier> _logger;
    public Notifier(
       INotificationProvider provider,
       IWebNotificationService webNotificationService,
       IServiceScopeFactory serviceScopeFactory, ILogger<Notifier> logger)
    {
        _provider = provider;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;

        _provider.Attach(NotificationType.LikePost, new List<INotificationService>()
        {
            webNotificationService,
        });
        _provider.Attach(NotificationType.CommentPost, new List<INotificationService>()
        {
            webNotificationService,
        });
        _provider.Attach(NotificationType.ReplyComment, new List<INotificationService>()
        {
            webNotificationService,
        });
        _provider.Attach(NotificationType.NewPost, new List<INotificationService>()
        {
            webNotificationService,
        });
        _provider.Attach(NotificationType.NewEvent, new List<INotificationService>()
        {
            webNotificationService,
        });
        _provider.Attach(NotificationType.JoinEvent, new List<INotificationService>()
        {
            webNotificationService,
        });
    }

    public async Task NotifyAsync(Notification notification, bool IsSaved)
    {
        if (IsSaved)
        {
            // persist notification into database
            using var scope = _serviceScopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await unitOfWork.NotificationRepository.AddAsync(notification);
            await unitOfWork.SaveChangesAsync();
        }

        _logger.LogInformation("[PUSH NOTIFICATION]: {0}", JsonSerializerUtils.Serialize(notification));
        await _provider.NotifyAsync(notification);
    }
}
