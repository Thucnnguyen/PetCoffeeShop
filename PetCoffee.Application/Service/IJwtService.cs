using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Service;

public interface IJwtService
{
	string GenerateJwtToken(Account account);


}
