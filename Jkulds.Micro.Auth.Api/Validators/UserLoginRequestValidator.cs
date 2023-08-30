using FluentValidation;
using Jkulds.Micro.Auth.Api.Controllers.Models.Api.Request;

namespace Jkulds.Micro.Auth.Api.Validators;

public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}