using System.ComponentModel;

namespace Jkulds.Micro.Auth.Api.Controllers.Models.Api.Request;

public class UserUpdatePasswordRequest
{
    [DefaultValue("P@ssword123!")]
    public string OldPassword { get; set; } = string.Empty;
    
    [DefaultValue("P@ssword!123")]
    public string NewPassword { get; set; } = string.Empty;

    [DefaultValue("P@ssword!123")]
    public string NewPasswordConfirmation { get; set; } = string.Empty;
}