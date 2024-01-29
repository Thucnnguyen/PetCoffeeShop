

using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class SubmittingEventRepository : BaseRepository<SubmittingEvent>, ISubmittingEventRepsitory
{
	private readonly ApplicationDbContext _dbContext;

	public SubmittingEventRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
