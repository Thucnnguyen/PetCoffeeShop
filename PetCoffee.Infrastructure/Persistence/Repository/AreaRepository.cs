
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class AreaRepository : BaseRepository<Area>, IAreaRepsitory
{
	private readonly ApplicationDbContext _dbContext;

	public AreaRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
