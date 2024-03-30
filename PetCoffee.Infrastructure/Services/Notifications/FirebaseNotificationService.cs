using FirebaseAdmin.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service.Notifications;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Infrastructure.Services.Notifications;

public class FirebaseNotificationService : IMobileNotificationService
{
	private readonly IServiceScopeFactory _serviceScopeFactory;
	private readonly ILogger<FirebaseNotificationService> _logger;

	public FirebaseNotificationService(IServiceScopeFactory serviceScopeFactory, ILogger<FirebaseNotificationService> logger)
	{
		_serviceScopeFactory = serviceScopeFactory;
		_logger = logger;
	}

	public async Task NotifyAsync(Domain.Entities.Notification notification)
	{
		// get device ids
		using var scope = _serviceScopeFactory.CreateScope();
		var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
		var notiAdapter = scope.ServiceProvider.GetRequiredService<INotificationAdapter>();

		var mess = new Message()
		{
			Topic = notification.AccountId.ToString(),
			Data = new Dictionary<string, string>()
			{
				{notification.Id.ToString(), JsonSerializerUtils.Serialize(notification)}
			},
			Notification = new Notification()
			{
				Title = notification.Title,
				Body = notification.Content,
			},
		};

		string response = await FirebaseMessaging.DefaultInstance.SendAsync(mess);

		_logger.LogInformation("[[MOBILE NOTIFICATION]] Handle firebase notification: {0}", notification.Id);
	}
}
