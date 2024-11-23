using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Common;
public class Helpers
{
    private static readonly Random Random = new Random();

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

    public static string GeneratePassword(int length = 8)
    {
        const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string allChars = upperChars + lowerChars + digits;

        char upper = upperChars[Random.Next(upperChars.Length)];
        char lower = lowerChars[Random.Next(lowerChars.Length)];
        char digit = digits[Random.Next(digits.Length)];

        char[] password = new char[length];
        password[0] = upper;
        password[1] = lower;
        password[2] = digit;

        for (int i = 3; i < length; i++)
        {
            password[i] = allChars[Random.Next(allChars.Length)];
        }

        return new string(password.OrderBy(_ => Random.Next()).ToArray());
    }
}
