namespace UniHub.Dto;

public class TokenDto
{
    public string RefreshToken { get; set; }

    public DateTime RefreshTokenExpiration { get; set; }

    public DateTime AccessTokenExpiration { get; set; }

    public string AccessToken { get; set; }
}