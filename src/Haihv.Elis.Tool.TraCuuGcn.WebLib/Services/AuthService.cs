using System.Net.Http.Json;
using Blazored.LocalStorage;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public interface IAuthService
{
    Task<AuthResult> LoginByChuSuDung(AuthChuSuDung authChuSuDung);
    Task Logout();
}

public class AuthService(
    IHttpClientFactory httpClientFactory,
    ILocalStorageService localStorage,
    AuthenticationStateProvider authStateProvider)
    : IAuthService
{
    private readonly JwtAuthStateProvider _jwtAuthStateProvider = (JwtAuthStateProvider)authStateProvider;  
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Endpoint");
    public async Task<AuthResult> LoginByChuSuDung(AuthChuSuDung authChuSuDung)
    {
        var response = await _httpClient.PostAsJsonAsync("/elis/auth", authChuSuDung);
        
        if (!response.IsSuccessStatusCode)
            return new AuthResult { Error = "Login failed" };

        var authResponse = await response.Content.ReadFromJsonAsync<AccessToken>();
        
        await localStorage.SetItemAsync("authToken", authResponse?.Token);
        await localStorage.SetItemAsync("refreshToken", authResponse?.RefreshToken);
        
        var authenticationState = await ((JwtAuthStateProvider)authStateProvider).GetAuthenticationStateAsync();
        
        ((JwtAuthStateProvider)authStateProvider).NotifyUserChanged(authenticationState);
        
        return new AuthResult { Success = true };
    }

    public async Task Logout()
    {
        await localStorage.RemoveItemAsync("authToken");
        await localStorage.RemoveItemAsync("refreshToken");
        _jwtAuthStateProvider.NotifyUserChanged(JwtAuthStateProvider.AnonymousUser);
    }
}

public record AuthResult(bool Success = false, string? Error = null);