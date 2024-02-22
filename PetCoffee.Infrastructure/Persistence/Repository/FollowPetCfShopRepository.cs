
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class FollowPetCfShopRepository : BaseRepository<FollowPetCfShop>, IFollowPetCfShopRepository
{
	public FollowPetCfShopRepository(DbContext dbContext) : base(dbContext)
	{
	}
}
