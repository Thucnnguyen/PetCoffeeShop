using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository
{
	public class ProductRepository : BaseRepository<Product>, IProductRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
	}


}
