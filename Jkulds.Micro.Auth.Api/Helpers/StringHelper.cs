namespace Jkulds.Micro.Auth.Api.Helpers;

public static class StringHelper
{
    public static string UnescapeString(string s)
    {
        var unescapedString = Uri.UnescapeDataString(s);
        
        return unescapedString.Length < s.Length ? unescapedString : s;
    }
}