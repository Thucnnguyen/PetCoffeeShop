

using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Persistence.Repository;

public interface IAccountRepository : IBaseRepository<Account>
{
	public Task<Account> GetUserByUserNameAndPassword(string userName, string passowrd);
}
