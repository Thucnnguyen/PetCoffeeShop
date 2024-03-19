using Microsoft.AspNetCore.SignalR;
using PetCoffee.Application.Persistence.Repository;

namespace PetCoffee.Infrastructure.SinalR.Notifications;

public class NotificationHub : Hub
{
	private readonly IConnectionManager _connectionManager;
	private readonly IUnitOfWork _unitOfWork;

	public NotificationHub(ConnectionManagerServiceResolver resolver, IUnitOfWork unitOfWork)
	{
		_connectionManager = resolver(typeof(NotificationConnectionManager));
		_unitOfWork = unitOfWork;
	}
	public async Task<string> Connect()
	{
		var httpContext = Context.GetHttpContext();
		var accId = long.Parse(httpContext.Request.Query["accountId"]);

		Console.WriteLine($"Connect method called for accountId: {accId}");

		var account = await _unitOfWork.AccountRepository.GetByIdAsync(accId);
		if (account == null)
		{
			throw new Exception("Account not found");
		}

		_connectionManager.KeepConnection(accId, Context.ConnectionId);
		return Context.ConnectionId;
	}

	public override async Task<string> OnConnectedAsync()
	{
		var httpContext = Context.GetHttpContext();
		var accId = long.Parse(httpContext.Request.Query["accountId"]);

		Console.WriteLine($"Connect method called for accountId: {accId}");

		var account = await _unitOfWork.AccountRepository.GetByIdAsync(accId);
		if (account == null)
		{
			throw new Exception("Account not found");
		}

		_connectionManager.KeepConnection(accId, Context.ConnectionId);
		return Context.ConnectionId;
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		var connectionId = Context.ConnectionId;
		_connectionManager.RemoveConnection(connectionId);
		return Task.CompletedTask;
	}
}
