using System.IdentityModel.Tokens.Jwt;
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
    private string? _lastKnownToken;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await localStorage.GetItemAsync<string>(AuthService.AccessTokenKey);
            
            // Kiểm tra xem token có thay đổi so với lần trước không
            if (token != _lastKnownToken)
            {
                Console.WriteLine("Token đã thay đổi, cập nhật trạng thái xác thực");
                _lastKnownToken = token;
            }
            
            if (string.IsNullOrWhiteSpace(token))
            {
                token = await _httpClientAuth.RefreshToken(localStorage);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    await localStorage.SetItemAsync(AuthService.AccessTokenKey, token);
                    _lastKnownToken = token;
                }
                else
                {
                    return CreateAnonymousUser();
                }
            }
            
            var jwtToken = _tokenHandler.ReadJwtToken(token);
            
            var expiry = jwtToken.ValidTo;
            if (expiry <= DateTime.UtcNow.AddSeconds(60))
            {
                token = await _httpClientAuth.RefreshToken(localStorage);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    await localStorage.SetItemAsync(AuthService.AccessTokenKey, token);
                    _lastKnownToken = token;
                    jwtToken = _tokenHandler.ReadJwtToken(token);
                }
            }
            
            if (jwtToken is null || jwtToken.ValidTo <= DateTime.UtcNow)
            {
                await localStorage.RemoveItemAsync(AuthService.AccessTokenKey);
                _lastKnownToken = null;
                return CreateAnonymousUser();
            }

            var claims = jwtToken.Claims.ToList();
            var identity = new ClaimsIdentity(claims, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi lấy trạng thái xác thực: {ex.Message}");
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