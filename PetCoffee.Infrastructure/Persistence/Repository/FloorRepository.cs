
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class FloorRepository : BaseRepository<Floor>, IFloorRepsitory
{
	private readonly ApplicationDbContext _dbContext;

	public FloorRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
