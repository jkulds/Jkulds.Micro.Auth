using Jkulds.Micro.Auth.Business.Services.AuthService.Dto;

namespace Jkulds.Micro.Auth.Business.Services.AuthService;

public interface IAuthService
{
    public Task<JwtTokenDto> RegisterUserAsync(UserRegistrationDto dto);
    public Task<JwtTokenDto> LoginUserAsync(UserLoginDto dto);
    public Task<JwtTokenDto> RefreshTokenAsync(JwtTokenDto dto);
    public Task UpdateUserPassword(UserUpdatePasswordDto dto);
    public Task<bool> ConfirmEmail(string token, string email);
    public Task<bool> SendResetPasswordLetter(string email);
    public Task<bool> ResetPassword(string email, string token, string newPassword);
}