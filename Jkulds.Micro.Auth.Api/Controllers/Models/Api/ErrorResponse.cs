using System.Text.Json;

namespace Jkulds.Micro.Auth.Api.Controllers.Models.Api;

public class ErrorResponse
{
    public ErrorResponse(string message)
    {
        Message = message;
    }
    
    public ErrorResponse(string message, string localizationKey)
    {
        Message = message;
        LocalizationKey = localizationKey;
    }
    
    public string Message { get; set; }
    public string LocalizationKey { get; set; } = string.Empty;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}