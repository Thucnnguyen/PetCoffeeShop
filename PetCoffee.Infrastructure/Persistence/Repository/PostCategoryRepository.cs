

using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class PostCategoryRepository : BaseRepository<PostCategory>, IPostCategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PostCategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
