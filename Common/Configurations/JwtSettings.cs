﻿namespace Common.Configurations;

public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int AccessTokenExp { get; set; }
    public int RefreshTokenExp { get; set; }
}
