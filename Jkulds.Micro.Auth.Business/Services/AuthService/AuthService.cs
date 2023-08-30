using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Jkulds.Micro.Auth.Business.Producers;
using Jkulds.Micro.Auth.Business.Services.Dto;
using Jkulds.Micro.Auth.Business.Services.Exceptions;
using Jkulds.Micro.Auth.Business.Services.Exceptions.Base;
using Jkulds.Micro.Auth.Data.Models.Identity;
using Jkulds.Micro.Base.Enum;
using Jkulds.Micro.MessageContracts.Notifications;
using Jkulds.Micro.Options.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Jkulds.Micro.Auth.Business.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly JwtOptions _jwtOptions;
    private readonly UserManager<ApiUser> _userManager;
    private readonly NotificationProducer _notificationProducer;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IOptions<JwtOptions> jwtOptions, 
        UserManager<ApiUser> userManager, 
        NotificationProducer notificationProducer, 
        ILogger<AuthService> logger)
    {
        _jwtOptions = jwtOptions.Value;
        _userManager = userManager;
        _notificationProducer = notificationProducer;
        _logger = logger;
    }

    public async Task<JwtTokenDto> RegisterUserAsync(UserRegistrationDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var emailExist = await _userManager.FindByEmailAsync(dto.Email);
        if (emailExist != null)
        {
            throw new UserRegistrationException("Email already exists");
        }

        var newUser = new ApiUser { Email = dto.Email, UserName = dto.Email };

        var isCreated = await _userManager.CreateAsync(newUser, dto.Password);
        if (!isCreated.Succeeded)
        {
            throw new UserRegistrationException("Something went wrong");
        }

        try
        {
            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            var nts = new NotificationToSend
            {
                Type = ENotificationType.Email,
                Status = ENotificationStatus.ToSend,
                Body = $"https//localhost:5001/api/ConfirmEmail?token={Uri.EscapeDataString(emailConfirmationToken)}&email={Uri.EscapeDataString(dto.Email)}",
                To = dto.Email,
                From = "h0lodiln@yandex.ru"
            };

            await _notificationProducer.SendNotification(nts);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error on send email notification");
        }

        var token = await GenerateTokenModelAndUpdateUserRefreshTokenAsync(newUser);

        return token;
    }

    public async Task<JwtTokenDto> LoginUserAsync(UserLoginDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            throw new UserLoginException("User not found");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!isPasswordValid)
        {
            throw new UserLoginException("Invalid password");
        }

        var token = await GenerateTokenModelAndUpdateUserRefreshTokenAsync(user);

        return token;
    }

    public async Task<JwtTokenDto> RefreshTokenAsync(JwtTokenDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var token = dto.AccessToken;

        var principal = GetPrincipalFromExpiredToken(token);
        if (principal == null)
        {
            throw new RefreshTokenException("Invalid token");
        }

        var username = principal.Identity!.Name!;

        var user = await _userManager.FindByNameAsync(username);
        var refreshToken = dto.RefreshToken;

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new RefreshTokenException("Invalid refresh token");
        }

        var newToken = await GenerateTokenModelAndUpdateUserRefreshTokenAsync(user);

        return newToken;
    }

    public async Task UpdateUserPassword(UserUpdatePasswordDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        var user = _userManager.Users.FirstOrDefault(x => x.Id == dto.UserId);
        if (user is null)
        {
            throw new UserUpdatePasswordException("User not found");
        }
        
        var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        if (!result.Succeeded)
        {
            throw new UserUpdatePasswordException(result.Errors.FirstOrDefault()?.Description ??
                                                  BaseExceptionMessages.SomethingWentWrong);
        }
    }

    public async Task<bool> SendResetPasswordLetter(string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(email);
        
        var user = await _userManager.FindByEmailAsync(email);

        ArgumentNullException.ThrowIfNull(user);
        
        try
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var nts = new NotificationToSend
            {
                Type = ENotificationType.Email,
                Status = ENotificationStatus.ToSend,
                Body = $"https//localhost:5001/api/ResetPassword?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(email)}",
                To = email,
                From = "h0lodiln@yandex.ru"
            };

            await _notificationProducer.SendNotification(nts);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error on send email notification");
        }

        return true;
    }

    public async Task<bool> ResetPassword(string email, string token, string newPassword)
    {
        ArgumentException.ThrowIfNullOrEmpty(email);
        ArgumentException.ThrowIfNullOrEmpty(token);
        ArgumentException.ThrowIfNullOrEmpty(newPassword);

        var user = await _userManager.FindByEmailAsync(email);

        ArgumentNullException.ThrowIfNull(user);
        
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        
        return result.Succeeded;
    }

    public async Task<bool> ConfirmEmail(string token, string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(token);
        ArgumentException.ThrowIfNullOrEmpty(email);
        
        var user = await _userManager.FindByEmailAsync(email);
        
        ArgumentNullException.ThrowIfNull(user);

        await _userManager.ConfirmEmailAsync(user, token);

        return true;
    }

    private async Task<string> GenerateJwtToken(ApiUser apiUser)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var claimsIdentity = new ClaimsIdentity
        (
            new[]
            {
                new Claim("id", apiUser.Id.ToString()), 
                new Claim(JwtRegisteredClaimNames.Sub, apiUser.Email!),
                new Claim(JwtRegisteredClaimNames.Email, apiUser.Email!), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, apiUser.UserName!), 
                new Claim(ClaimTypes.Name, apiUser.UserName!)
            }
        );

        var userRoles = await _userManager.GetRolesAsync(apiUser);

        foreach (var role in userRoles)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        var symmetricKey = new SymmetricSecurityKey(_jwtOptions.KeyBytes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Issuer = _jwtOptions.Issuer,
            Expires = DateTime.UtcNow.Add(_jwtOptions.TokenExpire),
            SigningCredentials = new SigningCredentials(symmetricKey, _jwtOptions.Algorithm)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);

        return jwtToken;
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = _jwtOptions.GetTokenValidationParameters();

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(_jwtOptions.Algorithm, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[128];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    private async Task<JwtTokenDto> GenerateTokenModelAndUpdateUserRefreshTokenAsync(ApiUser apiUser)
    {
        try
        {
            var token = await GenerateJwtToken(apiUser);
            var refreshToken = GenerateRefreshToken();

            apiUser.RefreshToken = refreshToken;
            apiUser.RefreshTokenExpiryTime = DateTime.UtcNow.Add(_jwtOptions.RefreshTokenExpire);

            await _userManager.UpdateAsync(apiUser);

            var result = new JwtTokenDto { AccessToken = token, RefreshToken = refreshToken };

            return result;
        }
        catch (Exception e)
        {
            throw new RefreshTokenException(e.Message, e);
        }
    }
}