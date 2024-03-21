
namespace PetCoffee.Shared.Ultils;
using System.Security.Cryptography;

using System.Text;
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
    public static String HmacSHA256(string inputData, string key)
    {
        byte[] keyByte = Encoding.UTF8.GetBytes(key);
        byte[] messageBytes = Encoding.UTF8.GetBytes(inputData);
        using (var hmacsha256 = new HMACSHA256(keyByte))
        {
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            string hex = BitConverter.ToString(hashmessage);
            hex = hex.Replace("-", "").ToLower();
            return hex;
        }
    }
}
