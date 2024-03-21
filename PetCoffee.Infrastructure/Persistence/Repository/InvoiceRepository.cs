using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository
{
	public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public InvoiceRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
	}
}
