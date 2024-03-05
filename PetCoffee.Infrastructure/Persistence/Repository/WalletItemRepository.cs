
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class WalletItemRepository : BaseRepository<WalletItem>, IWalletItemRepository
{
	private readonly ApplicationDbContext _dbContext;

	public WalletItemRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
