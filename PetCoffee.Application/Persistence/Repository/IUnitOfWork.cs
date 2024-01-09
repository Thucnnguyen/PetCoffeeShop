

using System.Data;

namespace PetCoffee.Application.Persistence.Repository;

public interface IUnitOfWork
{
	Task<int> SaveChangesAsync();

	void Rollback();

	IDbTransaction BeginTransaction();

	IAccountRepository AccountRepository { get; }
	
}
