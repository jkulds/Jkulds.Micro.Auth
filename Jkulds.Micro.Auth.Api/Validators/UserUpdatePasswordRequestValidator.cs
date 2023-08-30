using FluentValidation;
using Jkulds.Micro.Auth.Api.Controllers.Models.Api.Request;

namespace Jkulds.Micro.Auth.Api.Validators;

public class UserUpdatePasswordRequestValidator : AbstractValidator<UserUpdatePasswordRequest>
{
    public UserUpdatePasswordRequestValidator()
    {
        RuleFor(x => x.OldPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty();
        RuleFor(x => x.NewPasswordConfirmation).Matches(x => x.NewPassword);
    }
}