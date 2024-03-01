
using FirebaseAdmin.Auth;

namespace PetCoffee.Application.Service;

public interface IFirebaseService
{
	public Task<UserRecord?> VerifyFirebaseToken(string firebaseToken);
}
