
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class AccountRepository : BaseRepository<Account>, IAccountRepository
{
	private readonly ApplicationDbContext _dbContext;
	public AccountRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}

	public IQueryable<Account> GetUserByUserNameAndPassword(string userName, string passowrd, bool? isActive = null)
	{
		throw new NotImplementedException();
	}
}
