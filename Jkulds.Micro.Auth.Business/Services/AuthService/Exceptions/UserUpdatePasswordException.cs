using Jkulds.Micro.Auth.Business.Services.AuthService.Exceptions.Base;

namespace Jkulds.Micro.Auth.Business.Services.AuthService.Exceptions;

public class UserUpdatePasswordException : AuthBaseException
{
    public UserUpdatePasswordException(string message, Exception inner) : base(message, inner)
    {
    }

    public UserUpdatePasswordException(string message) : base(message)
    {
    }
}