using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class SubmittingEventRepository : BaseRepository<SubmittingEvent>, ISubmittingEventRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SubmittingEventRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
