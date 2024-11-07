using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Common;
public class Helpers
{
    public static string GetPasswordHash(string s)
    {
        if (string.IsNullOrEmpty(s))
            return string.Empty;

        using var hashAlgorithm = SHA512.Create();
        var hash = hashAlgorithm.ComputeHash(Encoding.Unicode.GetBytes(s));

        return string.Concat(hash.Select(item => item.ToString("x2")));
    }

    public static bool IsEmailValid(string email)
    {
        return email != null && new Regex("^[0-9a-zA-Z]([.-]?\\w+)*@[0-9a-zA-Z]([.-]?[0-9a-zA-Z])*(\\.[0-9a-zA-Z]{2,4})+$").IsMatch(email);
    }

    public static bool IsPasswordValid(string password)
    {
        if (password.Length < 6)
        {
            return false;
        }
        if (!password.Any(char.IsUpper))
        {
            return false;
        }
        if (!password.Any(char.IsLower))
        {
            return false;
        }
        if (!password.Any(char.IsDigit))
        {
            return false;
        }

        return true;
    }

}
