

using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PetCoffee.Application.Service.Notifications;
using PetCoffee.Application.Service.Notifications.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.SinalR.Notifications;
using PetCoffee.Infrastructure.SinalR;

namespace PetCoffee.Infrastructure.Services.Notifications.Website.SignalR;

public class WebNotificationService : IWebNotificationService
{
	private readonly ILogger<WebNotificationService> _logger;
	private readonly IHubContext<NotificationHub> _notificationHubContext;
	private readonly IConnectionManager _connectionManager;
	private readonly IMapper _mapper;
	private readonly INotificationAdapter _notificationAdapter;
	private const string ReceiveNotificationFunctionName = "ReceiveNotification";


	public WebNotificationService(ILogger<WebNotificationService> logger, 
		IHubContext<NotificationHub> notificationHubContext, 
		IMapper mapper,
		ConnectionManagerServiceResolver connectionManagerServiceResolver,
		INotificationAdapter notificationAdapter)
	{
		_logger = logger;
		_notificationHubContext = notificationHubContext;
		_connectionManager = connectionManagerServiceResolver(typeof(NotificationConnectionManager)); ;
		_mapper = mapper;
		_notificationAdapter = notificationAdapter;
	}

	public async Task NotifyAsync(Notification notification)
	{
		var connections = _connectionManager.GetConnections(notification.AccountId);
		if (connections.Any())
		{
			foreach (var connection in connections)
			{
				var notificationModel = await _notificationAdapter.ToWebNotification(notification, connection);
				await _notificationHubContext.Clients.Client(connection).SendAsync(ReceiveNotificationFunctionName, notificationModel);
			}
		}

		_logger.LogInformation($"[WEB NOTIFICATION] Send notification: {0}", notification.Id);
	}
}
