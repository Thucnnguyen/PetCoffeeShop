
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using PetCoffee.Infrastructure.Persistence.Context;
using PetCoffee.Shared.Ultils;
using System.Data.Entity;

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
		var user =  await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Email.Equals(email) 
															&& a.IsActive
															&& a.LoginMethod == LoginMethod.UserNamePass);
		if(user == null)
		{
			throw new ApiException(ResponseCode.LoginFailed);
		}
		if(!HashHelper.CheckHashPwd(passowrd, user.Password))
		{
			throw new ApiException(ResponseCode.LoginFailed);
		}
		return user;
	}
}
