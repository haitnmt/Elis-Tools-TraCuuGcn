using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public class JwtAuthStateProvider(ILocalStorageService localStorage) : AuthenticationStateProvider
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetItemAsync<string>("authToken");
        
        if (string.IsNullOrWhiteSpace(token) || !_tokenHandler.CanReadToken(token))
            return CreateAnonymousUser();
        
        try
        {
            var jwtToken = _tokenHandler.ReadJwtToken(token);
            var expiry = jwtToken.ValidTo;
            
            if (expiry < DateTime.UtcNow)
            {
                await localStorage.RemoveItemAsync("authToken");
                return CreateAnonymousUser();
            }

            var claims = jwtToken.Claims.ToList();
            var identity = new ClaimsIdentity(claims, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            return CreateAnonymousUser();
        }
    }

    private static AuthenticationState CreateAnonymousUser() => 
        new(new ClaimsPrincipal(new ClaimsIdentity()));
    public static AuthenticationState AnonymousUser => CreateAnonymousUser();
    public void NotifyUserChanged(AuthenticationState state)
    {
        NotifyAuthenticationStateChanged(Task.FromResult(state));
    }
}