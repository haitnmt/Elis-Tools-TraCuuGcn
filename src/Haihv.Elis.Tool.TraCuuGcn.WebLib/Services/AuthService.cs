using System.Net.Http.Json;
using Blazored.LocalStorage;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public interface IAuthService
{
    Task<AuthResult> LoginByChuSuDung(AuthChuSuDung authChuSuDung);
    Task<bool> CheckAuthorByMaGcnElis(long maGcnElis);
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
        
        await SetLocalStorageAsync(authResponse, authChuSuDung.MaGcnElis, authChuSuDung.SoDinhDanh);
        
        var authenticationState = await ((JwtAuthStateProvider)authStateProvider).GetAuthenticationStateAsync();
        
        ((JwtAuthStateProvider)authStateProvider).NotifyUserChanged(authenticationState);
        
        return new AuthResult { Success = true };
    }
    
    private async Task SetLocalStorageAsync(AccessToken? accessToken = null, long maGcnElis = 0, string maDinhDanh = "")
    {
        if (accessToken is not null && maGcnElis > 0)
        {
            await localStorage.SetItemAsync("authToken", accessToken.Token);
            await localStorage.SetItemAsync("refreshToken", accessToken.RefreshToken);
            await localStorage.SetItemAsync("maGcnElis", maGcnElis);
            await localStorage.SetItemAsync("maDinhDanh", maDinhDanh);
        }
    }
    
    public async Task<bool> CheckAuthorByMaGcnElis(long maGcnElis)
    {
        if (maGcnElis <= 0)
            return false;
        var maGcn = await localStorage.GetItemAsync<long>("maGcnElis");
        return maGcn == maGcnElis;
    }
    public async Task Logout()
    {
        await localStorage.RemoveItemAsync("authToken");
        await localStorage.RemoveItemAsync("refreshToken");
        await localStorage.RemoveItemAsync("maGcnElis");
        _jwtAuthStateProvider.NotifyUserChanged(JwtAuthStateProvider.AnonymousUser);
    }
}

public record AuthResult(bool Success = false, string? Error = null);