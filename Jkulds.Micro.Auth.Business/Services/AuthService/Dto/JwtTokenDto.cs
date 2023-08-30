namespace Jkulds.Micro.Auth.Business.Services.Dto;

public class JwtTokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}