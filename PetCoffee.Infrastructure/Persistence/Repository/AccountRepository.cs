

using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class AccountRepository : BaseRepository<Account>, IAccountRepository
{
    private readonly ApplicationDbContext _dbContext;
    public AccountRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Account> GetUserByUserNameAndPassword(string email, string passowrd)
    {
        var user = await _dbContext.Accounts
                                    .FirstOrDefaultAsync(a => a.Email.Equals(email)
                                                        && (a.IsActive || a.IsVerify)
                                                        && a.LoginMethod == LoginMethod.UserNamePass);
        return user;
    }
}
