using Jkulds.Micro.Auth.Business.Services.AuthService.Exceptions.Base;

namespace Jkulds.Micro.Auth.Business.Services.AuthService.Exceptions;

public class UserLoginException : AuthBaseException
{
    public UserLoginException(string message) : base(message)
    {
    }
}