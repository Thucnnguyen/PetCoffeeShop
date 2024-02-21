
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class FollowEventRepository : BaseRepository<JoinEvent>, IFollowEventRepository
{
	private readonly ApplicationDbContext _dbContext;

	public FollowEventRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
