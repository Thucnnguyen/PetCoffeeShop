
namespace PetCoffee.Shared.Ultils;

public class TokenUltils
{
	private const string AllowedCharacters = "0123456789abcdfghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

	public static string GenerateOTPCode(int length)
	{
		var rand = new Random();

		var otp = string.Empty;

		for (var i = 0; i < length; i++)
		{
			otp += AllowedCharacters[rand.Next(0, AllowedCharacters.Length)];
		}

		return otp;
	}
	public static string GenerateCodeForOrder()
	{
		string prefix = "ORD-";  
		int year = DateTime.UtcNow.Year;
		int dayOfYear = DateTime.UtcNow.DayOfYear;
		string dayString = dayOfYear.ToString("000");  

		// Generate a random 5-digit number 
		Random random = new Random();
		int randomNum = random.Next(10000, 99999);

		return prefix + year + dayString + "-" + randomNum;
	}
}
