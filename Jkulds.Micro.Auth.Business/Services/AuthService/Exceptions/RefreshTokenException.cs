using Jkulds.Micro.Auth.Business.Services.AuthService.Exceptions.Base;

namespace Jkulds.Micro.Auth.Business.Services.AuthService.Exceptions;

public class RefreshTokenException : AuthBaseException
{
    public RefreshTokenException(string message, Exception inner) : base(message, inner)
    {
    }

    public RefreshTokenException(string message) : base(message)
    {
    }
}