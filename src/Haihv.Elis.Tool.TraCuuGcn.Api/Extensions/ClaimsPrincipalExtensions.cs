using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Haihv.Elis.Tool.TraCuuGcn.Models;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    private static List<string> GetUserRoles(this ClaimsPrincipal user) 
    {
        var roles = user.FindFirstValue("realm_access");
        if (roles == null) return [];
    
        var rolesObject = JsonSerializer.Deserialize<JsonElement>(roles);
        if (rolesObject.TryGetProperty("roles", out var rolesArray))
        {
            return rolesArray.EnumerateArray()
                .Select(r => r.GetString())
                .Where(r => r != null)
                .ToList()!;
        }
    
        return [];
    }
    public static UserInfo GetUserInfo(this ClaimsPrincipal user)
    {
        return new UserInfo
        {
            DisplayName = user.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname") ?? string.Empty,
            PreferredUsername = user.FindFirstValue("preferred_username") ?? string.Empty,
            Email = user.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress") ?? string.Empty,
            EmailVerified = user.FindFirstValue("email_verified") == "true",
            Name = user.FindFirstValue("name") ?? string.Empty,
            Surname = user.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname") ?? string.Empty,
            Roles = user.GetUserRoles()
        };
    }
}