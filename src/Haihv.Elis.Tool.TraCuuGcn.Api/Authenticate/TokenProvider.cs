﻿using System.Security.Claims;
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
    public static string? GetMaDinhDanh(this ClaimsPrincipal claimsPrincipal)
    {
        var id = claimsPrincipal.FindFirst("SoDinhDanh")?.Value;
        if (!string.IsNullOrWhiteSpace(id)) return id;
        id = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        return string.IsNullOrWhiteSpace(id) ? claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Name)?.Value : id;
    }
    public static string? GetHoVaTen(this ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal.FindFirst("HoVaTen")?.Value ?? claimsPrincipal.FindFirst(JwtRegisteredClaimNames.GivenName)?.Value;
    
    public static string? GetIdentityType(this ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Typ)?.Value;
}