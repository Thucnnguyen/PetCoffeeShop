
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class JoinEventRepository : BaseRepository<JoinEvent>, IJoinEventRepository
{
    private readonly ApplicationDbContext _dbContext;

    public JoinEventRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
