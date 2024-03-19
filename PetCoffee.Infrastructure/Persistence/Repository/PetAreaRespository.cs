
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class PetAreaRespository : BaseRepository<PetArea>, IPetAreaRespository
{
	private readonly ApplicationDbContext _dbContext;
	public PetAreaRespository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
