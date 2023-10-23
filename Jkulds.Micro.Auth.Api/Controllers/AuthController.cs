using AutoMapper;
using FluentValidation;
using Jkulds.Micro.Auth.Api.Controllers.Models.Api;
using Jkulds.Micro.Auth.Api.Controllers.Models.Api.Request;
using Jkulds.Micro.Auth.Api.Controllers.Models.Api.Response;
using Jkulds.Micro.Auth.Api.Helpers;
using Jkulds.Micro.Auth.Business.Services.AuthService;
using Jkulds.Micro.Auth.Business.Services.AuthService.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jkulds.Micro.Auth.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(IAuthService authService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _authService = authService;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Регистрация нового пользователя
    /// </summary>
    [HttpPost(nameof(Register))]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request, [FromServices] IValidator<UserRegistrationRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponse("Invalid register request"));
        }

        var registrationModel = _mapper.Map<UserRegistrationDto>(request);

        var tokenDto = await _authService.RegisterUserAsync(registrationModel);

        var result = _mapper.Map<TokenResponse>(tokenDto);
        
        return Ok(result);
    }

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    [HttpPost(nameof(Login))]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request, [FromServices] IValidator<UserLoginRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponse("Invalid login request"));
        }

        var loginModel = _mapper.Map<UserLoginDto>(request);

        var tokenDto = await _authService.LoginUserAsync(loginModel);

        var result = _mapper.Map<TokenResponse>(tokenDto);
        
        return Ok(result);
    }

    /// <summary>
    /// Обновление токена пользователя
    /// </summary>
    [HttpPost(nameof(RefreshToken))]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, [FromServices] IValidator<RefreshTokenRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponse("Invalid refresh token request"));
        }

        var tokenDto = _mapper.Map<JwtTokenDto>(request);

        var newToken = await _authService.RefreshTokenAsync(tokenDto);

        var result = _mapper.Map<TokenResponse>(newToken);
        
        return Ok(result);
    }

    /// <summary>
    /// Обновляет пароль пользователя
    /// </summary>
    [Authorize]
    [HttpPut(nameof(UpdateUserPassword))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserPassword(UserUpdatePasswordRequest request, [FromServices] IValidator<UserUpdatePasswordRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponse("Invalid update password request"));
        }

        
        var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
        
        var dto = _mapper.Map<UserUpdatePasswordDto>(request);

        dto.UserId = Guid.Parse(userId);
        await _authService.UpdateUserPassword(dto);

        return NoContent();
    }

    /// <summary>
    /// Проверка авторизации
    /// </summary>
    [Authorize]
    [HttpPost(nameof(TestAuth))]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> TestAuth() { return await Task.FromResult(Ok()); }

    /// <summary>
    /// Подтверждение почты
    /// </summary>
    [HttpGet(nameof(ConfirmEmail))]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string email)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
        {
            return BadRequest(new ErrorResponse("Invalid request"));
        }

        var result = await _authService.ConfirmEmail(token, email);

        if (result)
        {
            return Ok();
        }

        return BadRequest(new ErrorResponse("Something went wrong"));
    }

    [HttpGet(nameof(SendResetPasswordLetter))]
    public async Task<IActionResult> SendResetPasswordLetter([FromQuery] string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest(new ErrorResponse("Invalid request"));
        }

        var result = await _authService.SendResetPasswordLetter(email);

        if (result)
        {
            return Ok();
        }

        return BadRequest(new ErrorResponse("Something went wrong"));
    }
    
    [HttpPost(nameof(ResetPassword))]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        request.Token = StringHelper.UnescapeString(request.Token);
        request.Email = StringHelper.UnescapeString(request.Email);
        
        var result = await _authService.ResetPassword(request.Email, request.Token, request.NewPassword);
        if (result)
        {
            return Ok();
        }

        return BadRequest(new ErrorResponse("Something went wrong"));
    }
}