using Jkulds.Micro.Auth.Business.Services.Exceptions.Base;

namespace Jkulds.Micro.Auth.Business.Services.Exceptions;

public class UserLoginException : AuthBaseException
{
    public UserLoginException(string message) : base(message)
    {
    }
}