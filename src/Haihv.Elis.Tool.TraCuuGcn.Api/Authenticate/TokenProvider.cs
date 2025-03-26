using System.Security.Claims;
using System.Text;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;

public sealed class TokenProvider(
    string secretKey = "Jwt:SecretKey",
    string issuer = "Jwt:Issuer",
    string audience = "Jwt:Audience",
    int expiryMinutes = 60)
{
    public AccessToken GenerateToken(AuthChuSuDung chuSuDung)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenHandler = new JsonWebTokenHandler();
        var id = Guid.CreateVersion7().ToString();
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, id),
            new("SoDinhDanh", chuSuDung.SoDinhDanh),
            new("HoVaTen", chuSuDung.HoVaTen)
        };
        var expires = DateTime.UtcNow.AddMinutes(expiryMinutes);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        return new AccessToken(tokenHandler.CreateToken(tokenDescriptor), id, string.Empty, expires);
    }
}

public static class TokenExtensions
{
    private static string? GetClaimValue(this HttpContext context, string claimType)
        => context.User.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
    private static string? GetClaimValue(this ClaimsPrincipal claimsPrincipal, string claimType)
        => claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
    public static string? GetMaDinhDanh(this ClaimsPrincipal claimsPrincipal)
    {
        var id = claimsPrincipal.FindFirst("SoDinhDanh")?.Value;
        if (!string.IsNullOrWhiteSpace(id)) return id;
        id = claimsPrincipal.GetSamAccountName();
        return string.IsNullOrWhiteSpace(id) ? claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value : id;
    }
    public static string? GetHoVaTen(this ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal.FindFirst("HoVaTen")?.Value ?? claimsPrincipal.FindFirst(JwtRegisteredClaimNames.GivenName)?.Value;

    public static string? GetIdentityType(this ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Typ)?.Value;
    public static string GetUserPrincipalName(this HttpContext context)
        => context.GetClaimValue("UserPrincipalName") ?? string.Empty;
    public static string GetUserPrincipalName(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.GetClaimValue("UserPrincipalName") ?? string.Empty;
    public static string GetSamAccountName(this HttpContext context)
    {
        var samAccountName = context.GetClaimValue("SamAccountName");
        if (!string.IsNullOrWhiteSpace(samAccountName)) return samAccountName;
        var userPrincipalName = context.GetClaimValue("UserPrincipalName");
        samAccountName = userPrincipalName?.Split('@')[0];
        return samAccountName ?? string.Empty;
    }
    public static string GetSamAccountName(this ClaimsPrincipal claimsPrincipal)
    {
        var samAccountName = claimsPrincipal.GetClaimValue("SamAccountName");
        if (!string.IsNullOrWhiteSpace(samAccountName)) return samAccountName;
        var userPrincipalName = claimsPrincipal.GetClaimValue("UserPrincipalName");
        samAccountName = userPrincipalName?.Split('@')[0];
        return samAccountName ?? string.Empty;
    }

}