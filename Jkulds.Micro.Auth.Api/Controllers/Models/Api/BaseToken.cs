using System.ComponentModel.DataAnnotations;

namespace Jkulds.Micro.Auth.Api.Controllers.Models.Api;

public abstract class BaseToken
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;
    
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}