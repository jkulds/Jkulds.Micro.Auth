using Jkulds.Micro.Auth.Business.Services.Exceptions.Base;

namespace Jkulds.Micro.Auth.Business.Services.Exceptions;

public class UserRegistrationException : AuthBaseException
{
    public UserRegistrationException(string message) : base(message)
    {
    }
}