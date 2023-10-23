using Microsoft.AspNetCore.Identity;

namespace Jkulds.Micro.Auth.Data.Models.Identity;

public class ApiUser : IdentityUser<Guid>
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateOnly? DateOfBirthdate { get; set; }
}