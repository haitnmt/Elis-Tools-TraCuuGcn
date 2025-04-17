using System.Security.Claims;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static List<string> GetUserRoles(this ClaimsPrincipal user)
    {
        return user.Claims
            .Where(c => c.Type is "role" or "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
            .Select(c => c.Value)
            .ToList();
    }
    
    public static UserInfo GetUserInfo(this ClaimsPrincipal user)
    {
        return new UserInfo
        {
            DisplayName = user.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value ?? string.Empty,
            PreferredUsername = user.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value ?? string.Empty,
            Email = user.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? string.Empty,
            EmailVerified = user.Claims.FirstOrDefault(c => c.Type == "email_verified")?.Value == "true",
            Name = user.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? string.Empty,
            FamilyName = user.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value ?? string.Empty,
        };
    }
}