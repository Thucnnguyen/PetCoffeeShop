
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class TransactionItemRepository : BaseRepository<TransactionItem>, ITransactionItemRepository
{
	private readonly ApplicationDbContext _dbContext;

	public TransactionItemRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
