
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class SubmittingEventFieldRepository : BaseRepository<SubmittingEventField>, ISubmittingEventFieldRepository
{
	private readonly ApplicationDbContext _dbContext;

	public SubmittingEventFieldRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
