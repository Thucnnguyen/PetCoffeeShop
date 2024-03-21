

using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class AccountShopRespository : BaseRepository<AccountShop>, IAccountShopRespository
{
    private readonly ApplicationDbContext _dbContext;
    public AccountShopRespository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
