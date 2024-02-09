

using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class PostPetCoffeeShopRepository : BaseRepository<PostPetCoffeeShop>, IPostCoffeeShopRepository
{
	private readonly ApplicationDbContext _dbContext;
	public PostPetCoffeeShopRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
