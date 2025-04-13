using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public class JwtAuthStateProvider(IHttpClientFactory httpClientFactory, 
    ILocalStorageService localStorage)
    : AuthenticationStateProvider
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly HttpClient _httpClientAuth = httpClientFactory.CreateClient("AuthEndpoint");

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await localStorage.GetItemAsync<string>(AuthService.AccessTokenKey);
            if (string.IsNullOrWhiteSpace(token))
            {
                token = await _httpClientAuth.RefreshToken();
                await localStorage.SetItemAsync(AuthService.AccessTokenKey, token);
            }
            
            var jwtToken = _tokenHandler.ReadJwtToken(token);
            
            var expiry = jwtToken.ValidTo;
            if (expiry <= DateTime.UtcNow.AddSeconds(60))
            {
                token = await _httpClientAuth.RefreshToken();
                await localStorage.SetItemAsync(AuthService.AccessTokenKey, token);
            }
            
            jwtToken = _tokenHandler.ReadJwtToken(token);
            
            if (jwtToken is null || jwtToken.ValidTo <= DateTime.UtcNow)
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