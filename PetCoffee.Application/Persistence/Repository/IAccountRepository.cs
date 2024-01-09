

using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Persistence.Repository;

public interface IAccountRepository : IBaseRepository<Account>
{
	public IQueryable<Account> GetUserByUserNameAndPassword(string userName, string passowrd, bool? isActive = null);
}
