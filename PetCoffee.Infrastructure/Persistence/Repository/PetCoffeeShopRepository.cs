
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class PetCoffeeShopRepository : BaseRepository<PetCoffeeShop>, IPetCoffeeShopRepository
{
	private readonly ApplicationDbContext _dbContext;

	public PetCoffeeShopRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
