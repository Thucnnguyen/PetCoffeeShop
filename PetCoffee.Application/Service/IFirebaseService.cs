
namespace PetCoffee.Application.Service;

public interface IFirebaseService
{
	public Task<bool> VerifyFirebaseToken(string firebaseToken);
}
