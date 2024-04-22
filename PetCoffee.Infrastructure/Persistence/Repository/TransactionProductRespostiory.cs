
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class TransactionProductRespostiory : BaseRepository<TransactionProduct>, ITransactionProductRespostiory
{
	private readonly ApplicationDbContext _dbContext;

	public TransactionProductRespostiory(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
