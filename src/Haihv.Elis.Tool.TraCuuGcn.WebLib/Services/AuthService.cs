using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public interface IAuthService
{
    Task<AuthResult> LoginByChuSuDung(AuthChuSuDung authChuSuDung);
    Task<AuthResult> LoginUser(AuthUser authUser);
    Task<bool> CheckAuthorBySerialElis(string? serial);
    Task<string> GetSerialElis();
    Task SetSerialElis(string serial);
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
    private readonly HttpClient _httpClientAuth = httpClientFactory.CreateClient("AuthEndpoint");
    private const string SerialKey = "serial";
    private const string MaDinhDanhKey = "maDinhDanh";
    private const string AuthenTokenKey = "authToken";
    private const string RefreshTokenKey = "refreshToken";
    public async Task<AuthResult> LoginByChuSuDung(AuthChuSuDung authChuSuDung)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/elis/auth", authChuSuDung);
            
            var authResponse = await response.Content.ReadFromJsonAsync<Response<AccessToken>>();

            if (authResponse?.Value is null)
                return new AuthResult { Error = authResponse?.ErrorMsg };
                
            await SetLocalStorageAsync(authResponse.Value, authChuSuDung.Serial, authChuSuDung.SoDinhDanh);
        
            var authenticationState = await ((JwtAuthStateProvider)authStateProvider).GetAuthenticationStateAsync();
        
            ((JwtAuthStateProvider)authStateProvider).NotifyUserChanged(authenticationState);
        
            return new AuthResult { Success = true };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new AuthResult { Error = e.Message };
        }

    }
    public async Task<AuthResult> LoginUser(AuthUser authUser)
    {
        try
        {
            var response = await _httpClientAuth.PostAsJsonAsync("/login", authUser);
            
            var authResponse = await response.Content.ReadFromJsonAsync<Response<AccessToken>>();

            if (authResponse?.Value is null)
                return new AuthResult { Error = authResponse?.ErrorMsg };
            await SetLocalStorageAsync(authResponse.Value);

            var authenticationState = await ((JwtAuthStateProvider)authStateProvider).GetAuthenticationStateAsync();

            ((JwtAuthStateProvider)authStateProvider).NotifyUserChanged(authenticationState);

            return new AuthResult { Success = true };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new AuthResult { Error = e.Message };
        }
    }
    
    private async Task SetLocalStorageAsync(AccessToken? accessToken = null, string? serial = null, string maDinhDanh = "")
    {
        if (accessToken is not null)
        {
            await localStorage.SetItemAsync(AuthenTokenKey, accessToken.Token);
            await localStorage.SetItemAsync(RefreshTokenKey, accessToken.RefreshToken);  
            if (!string.IsNullOrWhiteSpace(serial))
                await localStorage.SetItemAsync(SerialKey, serial);
            if (!string.IsNullOrWhiteSpace(maDinhDanh))
                await localStorage.SetItemAsync(MaDinhDanhKey, maDinhDanh);
        }
    }
    
    public async Task<bool> CheckAuthorBySerialElis(string? serial)
    {
        var authenticationState = await ((JwtAuthStateProvider)authStateProvider).GetAuthenticationStateAsync();
        if (authenticationState.User.Identity is not ClaimsIdentity claimsIdentity)
            return false;
        if (claimsIdentity.FindFirst(JwtRegisteredClaimNames.Typ)?.Value.ToLower() == "ldap")
            return true;
        if (string.IsNullOrWhiteSpace(serial))
            return false;
        var serialInLocalStorage = await localStorage.GetItemAsync<string>(SerialKey);
        return serialInLocalStorage == serial;
    }
    
    public async Task<string> GetSerialElis()
    {
        return await localStorage.GetItemAsync<string>(SerialKey) ?? string.Empty;
    }
    
    public async Task SetSerialElis(string serial)
    {
        await localStorage.SetItemAsync(SerialKey, serial);
    }
    public async Task Logout()
    {
        await localStorage.RemoveItemAsync(AuthenTokenKey);
        await localStorage.RemoveItemAsync(RefreshTokenKey);
        await localStorage.RemoveItemAsync(MaDinhDanhKey);
        _jwtAuthStateProvider.NotifyUserChanged(JwtAuthStateProvider.AnonymousUser);
    }
}

public record AuthResult(bool Success = false, string? Error = null);
public record AuthUser(string Username, string Password, bool RememberMe = false);