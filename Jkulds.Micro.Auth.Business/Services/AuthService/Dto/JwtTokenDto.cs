namespace Jkulds.Micro.Auth.Business.Services.AuthService.Dto;

public class JwtTokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}