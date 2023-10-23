namespace Jkulds.Micro.Auth.Business.Services.AuthService.Dto;

public class UserUpdatePasswordDto
{
    public Guid UserId { get; set; }
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}