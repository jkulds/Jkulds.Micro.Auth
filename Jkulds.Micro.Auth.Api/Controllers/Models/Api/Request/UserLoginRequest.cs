using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Jkulds.Micro.Auth.Api.Controllers.Models.Api.Request;

public class UserLoginRequest
{
    [Required]
    [DefaultValue("test@test.test")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DefaultValue("P@ssword123!")]
    public string Password { get; set; } = string.Empty;
}