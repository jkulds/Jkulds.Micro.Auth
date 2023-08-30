using FluentValidation;
using Jkulds.Micro.Auth.Api.Controllers.Models.Api.Request;

namespace Jkulds.Micro.Auth.Api.Validators;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.AccessToken).NotEmpty();
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}