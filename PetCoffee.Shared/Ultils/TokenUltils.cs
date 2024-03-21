
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
}
