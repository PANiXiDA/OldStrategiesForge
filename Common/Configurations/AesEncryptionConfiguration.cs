namespace Common.Configurations;
public class AesEncryptionConfiguration
{
    public string SecretKey { get; set; } = string.Empty;
    public int TokenLifeTimeDays { get; set; }
}
