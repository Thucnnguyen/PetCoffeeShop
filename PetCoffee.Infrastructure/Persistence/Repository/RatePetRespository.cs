
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class RatePetRespository : BaseRepository<RatePet>, IRatePetRepository
{
    private readonly ApplicationDbContext _dbContext;
    public RatePetRespository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
