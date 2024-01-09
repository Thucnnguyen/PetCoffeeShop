
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Infrastructure.Persistence.Context;
using System.Data;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext _dbContext;
	private bool _disposed;
	private readonly IServiceScopeFactory _scopeFactory;



	public UnitOfWork(ApplicationDbContext dbContext, IServiceScopeFactory scopeFactory) 
	{
		_dbContext = dbContext;
		_scopeFactory = scopeFactory;
	}

	public IDbTransaction BeginTransaction()
	{
		var transaction = _dbContext.Database.BeginTransaction();
		return transaction.GetDbTransaction();
	}

	public void Rollback()
	{
		foreach (var entry in _dbContext.ChangeTracker.Entries())
			switch (entry.State)
			{
				case EntityState.Added:
					entry.State = EntityState.Detached;
					break;
			}
	}

	public async Task<int> SaveChangesAsync()
	{
		return await _dbContext.SaveChangesAsync();
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
			if (disposing)
				_dbContext.Dispose();

		_disposed = true;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	private IAccountRepository? _accountRepository;
	public IAccountRepository AccountRepository => _accountRepository ??= new AccountRepository(_dbContext);

}
