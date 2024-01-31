
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
{
	private readonly ApplicationDbContext _dbContext;

	public WalletRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
