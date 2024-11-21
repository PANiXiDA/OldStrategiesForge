using Common.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Tools.Encryption;
public class AesEncryption
{
    private readonly byte[] _key;
    private readonly TimeSpan _tokenLifeTime;
    private readonly ILogger<AesEncryption> _logger;

    public AesEncryption(IOptions<AesEncryptionConfiguration> configuration, ILogger<AesEncryption> logger)
    {
        _key = Encoding.UTF8.GetBytes(configuration.Value.SecretKey);
        _tokenLifeTime = TimeSpan.FromDays(configuration.Value.TokenLifeTimeDays);
        _logger = logger;
    }

    public string Encrypt<T>(T obj)
    {
        if (obj == null)
        {
            _logger.LogError("Encryption failed: object is null.");
            throw new ArgumentNullException(nameof(obj));
        }

        string json = JsonSerializer.Serialize(obj);
        _logger.LogInformation("Object serialized to JSON: {Json}", json);

        var tokenData = new TokenData
        {
            Timestamp = DateTime.UtcNow,
            Data = json
        };

        string tokenJson = JsonSerializer.Serialize(tokenData);
        byte[] plainText = Encoding.UTF8.GetBytes(tokenJson);
        _logger.LogInformation("Token data serialized to JSON and converted to bytes.");

        byte[] cipherText = EncryptData(plainText);
        _logger.LogInformation("Data encrypted successfully.");

        return Convert.ToBase64String(cipherText);
    }

    private byte[] EncryptData(byte[] plainText)
    {
        using Aes aes = Aes.Create();
        aes.GenerateIV();
        aes.Key = _key;
        byte[] iv = aes.IV;

        _logger.LogInformation("AES IV generated: {IV}", Convert.ToBase64String(iv));

        using ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using MemoryStream ms = new MemoryStream();
        ms.Write(iv, 0, iv.Length);

        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        {
            cs.Write(plainText, 0, plainText.Length);
        }

        _logger.LogInformation("Encryption process completed.");

        return ms.ToArray();
    }

    public T Decrypt<T>(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            _logger.LogError("Decryption failed: token is null or empty.");
            throw new ArgumentNullException(nameof(token));
        }

        byte[] cipherBytes = Convert.FromBase64String(token);
        _logger.LogInformation("Token converted from Base64 to byte array.");

        byte[] plainText = DecryptData(cipherBytes);
        string tokenJson = Encoding.UTF8.GetString(plainText);

        var tokenData = JsonSerializer.Deserialize<TokenData>(tokenJson);
        _logger.LogInformation("Token data deserialized from JSON.");

        if (tokenData == null)
        {
            _logger.LogError("Invalid token data.");
            throw new InvalidOperationException("Invalid token data.");
        }

        if (DateTime.UtcNow - tokenData.Timestamp > _tokenLifeTime)
        {
            _logger.LogError("Token has expired.");
            throw new InvalidOperationException("Token has expired.");
        }

        T? result = JsonSerializer.Deserialize<T>(tokenData.Data);
        if (result == null)
        {
            _logger.LogError("Deserialized object is null.");
            throw new InvalidOperationException("Deserialized object is null.");
        }

        _logger.LogInformation("Decryption successful.");
        return result;
    }

    private byte[] DecryptData(byte[] cipherText)
    {
        using Aes aes = Aes.Create();
        aes.Key = _key;

        byte[] iv = new byte[16];
        Array.Copy(cipherText, 0, iv, 0, iv.Length);
        aes.IV = iv;

        _logger.LogInformation("AES IV extracted from ciphertext.");

        byte[] actualCipherText = new byte[cipherText.Length - iv.Length];
        Array.Copy(cipherText, iv.Length, actualCipherText, 0, actualCipherText.Length);

        using ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using MemoryStream ms = new MemoryStream(actualCipherText);
        using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using MemoryStream resultStream = new MemoryStream();
        cs.CopyTo(resultStream);

        _logger.LogInformation("Decryption process completed.");
        return resultStream.ToArray();
    }
}