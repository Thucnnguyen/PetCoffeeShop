
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Infrastructure.Persistence.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
	private readonly ICurrentAccountService _currentAccountService;
	private readonly ILogger<AuditableEntitySaveChangesInterceptor> _logger;

	public AuditableEntitySaveChangesInterceptor(ICurrentAccountService currentAccountService, 
		ILogger<AuditableEntitySaveChangesInterceptor> logger)
	{
		_currentAccountService = currentAccountService;
		_logger = logger;
	}
	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
		CancellationToken cancellationToken = new())
	{
		await UpdateEntities(eventData.Context);
		return await base.SavingChangesAsync(eventData, result, cancellationToken);
	}
	private async Task UpdateEntities(DbContext? context)
	{
		if (context == null) return;
		var currentAccount = await _currentAccountService.GetCurrentAccount();
		var currentAccountId = currentAccount?.Id;

		foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
		{
			if (entry.State == EntityState.Added)
			{
				entry.Entity.CreatedById = currentAccountId;
				entry.Entity.CreatedAt = DateTime.UtcNow;
			}

			if (entry.State == EntityState.Added || entry.HasChangedOwnedEntities())
			{
				entry.Entity.CreatedById = currentAccountId;
				entry.Entity.CreatedAt = DateTime.UtcNow;
			}
		}
	}
	
}
public static class Extensions
{
	public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
		entry.References.Any(r =>
			r.TargetEntry != null &&
			r.TargetEntry.Metadata.IsOwned() &&
			r.TargetEntry.State == EntityState.Added );
}
