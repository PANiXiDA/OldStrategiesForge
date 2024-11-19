namespace ProfileService.Dto;

public class TokensDto
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExp { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public DateTime AccessTokenExp { get; set; }
}
