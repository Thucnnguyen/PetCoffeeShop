
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Service;

public interface ICurrentAccountService
{
    public Task<Account?> GetCurrentAccount();
    public Task<Account> GetRequiredCurrentAccount();
}
