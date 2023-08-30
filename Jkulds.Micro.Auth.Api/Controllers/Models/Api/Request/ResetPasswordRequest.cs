using System.ComponentModel.DataAnnotations;

namespace Jkulds.Micro.Auth.Api.Controllers.Models.Api.Request;

public class ResetPasswordRequest
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Token { get; set; }
    [Required]
    public string NewPassword { get; set; }
}