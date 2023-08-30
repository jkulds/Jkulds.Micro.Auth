namespace Jkulds.Micro.Auth.Business.Services.Dto;

public class UserUpdatePasswordDto
{
    public Guid UserId { get; set; }
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}