namespace UniHub.Dto;

public class TokenDto
{
    public string RefershToken { get; set; }

    public DateTime RefershTokenExpires { get; set; }

    public string AccessToken { get; set; }
}