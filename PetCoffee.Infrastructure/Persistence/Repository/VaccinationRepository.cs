
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class VaccinationRepository : BaseRepository<Vaccination>, IVaccinationRepository
{
	private readonly ApplicationDbContext _dbContext;

	public VaccinationRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
