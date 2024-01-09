
namespace PetCoffee.Shared.Ultils;
using BC = BCrypt.Net.BCrypt;
public class HashHelper
{
	public static string HashPassword(string password)
	{

		string pwdHash = BC.HashPassword(password);
		return pwdHash;
	}

	public static bool CheckHashPwd(string input, string hashPwd)
	{
		bool verified = BC.Verify(input, hashPwd);
		return verified;
	}
}
