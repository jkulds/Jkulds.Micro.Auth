using Jkulds.Micro.Auth.Business.Services.AuthService.Exceptions.Base;

namespace Jkulds.Micro.Auth.Business.Services.AuthService.Exceptions;

public class UserRegistrationException : AuthBaseException
{
    public UserRegistrationException(string message) : base(message)
    {
    }
}