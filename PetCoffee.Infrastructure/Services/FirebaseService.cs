
using FirebaseAdmin.Auth;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Service;

namespace PetCoffee.Infrastructure.Services;

public class FirebaseService : IFirebaseService
{

	public FirebaseService()
	{
	}

	public async Task<UserRecord?> VerifyFirebaseToken(string firebaseToken)
	{
		try
		{
			var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(firebaseToken);
			string uid = decodedToken.Uid;

			// Lấy thông tin người dùng từ UID
			var user = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
			return user;

		}catch (Exception ex)
		{
			throw new ApiException(ResponseCode.FirebaseTokenNotValid);
		}



	}
}
