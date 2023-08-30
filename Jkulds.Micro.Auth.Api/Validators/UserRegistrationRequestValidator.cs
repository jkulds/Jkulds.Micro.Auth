using FluentValidation;
using FluentValidation.Results;
using Jkulds.Micro.Auth.Api.Controllers.Models.Api.Request;

namespace Jkulds.Micro.Auth.Api.Validators;

public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequest>
{
    public UserRegistrationRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Name).Length(2, 30);
        RuleFor(x => x.Password).NotEmpty();
    }

    public override ValidationResult Validate(ValidationContext<UserRegistrationRequest> context)
    {
        return context.InstanceToValidate == null 
            ? new ValidationResult(new []{ new ValidationFailure("request", "request cannot be null")}) 
            : base.Validate(context);
    }
}