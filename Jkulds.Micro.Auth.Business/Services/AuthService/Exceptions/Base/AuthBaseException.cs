namespace Jkulds.Micro.Auth.Business.Services.AuthService.Exceptions.Base;

public abstract class AuthBaseException : Exception
{
    protected AuthBaseException(string message, Exception inner) : base(message, inner)
    {
        
    }
    
    protected AuthBaseException(string message) : base(message)
    {
        
    }
}