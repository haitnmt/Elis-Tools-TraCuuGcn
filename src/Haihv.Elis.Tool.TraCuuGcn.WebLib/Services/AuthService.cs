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
    Task<bool> CheckAuthorByMaGcnElis(long maGcnElis);
    Task<long> GetMaGcnElis();
    Task SetMaGcnElis(long maGcnElis);
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
    private const string MaGcnElisKey = "maGcnElis";
    private const string MaDinhDanhKey = "maDinhDanh";
    private const string AuthenTokenKey = "authToken";
    private const string RefreshTokenKey = "refreshToken";
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
    public async Task<AuthResult> LoginUser(AuthUser authUser)
    {
        try
        {
            var response = await _httpClientAuth.PostAsJsonAsync("/login", authUser);
            
            var authResponse = await response.Content.ReadFromJsonAsync<Response<AccessToken>>();

            if (authResponse?.Value is null)
                return new AuthResult { Error = authResponse.ErrorMsg };
            await SetLocalStorageAsync(authResponse.Value);

            var authenticationState = await ((JwtAuthStateProvider)authStateProvider).GetAuthenticationStateAsync();

            ((JwtAuthStateProvider)authStateProvider).NotifyUserChanged(authenticationState);

            return new AuthResult { Success = true };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return new AuthResult { Error = "Login failed" };
    }
    
    private async Task SetLocalStorageAsync(AccessToken? accessToken = null, long maGcnElis = 0, string maDinhDanh = "")
    {
        if (accessToken is not null)
        {
            await localStorage.SetItemAsync(AuthenTokenKey, accessToken.Token);
            await localStorage.SetItemAsync(RefreshTokenKey, accessToken.RefreshToken);  
            if (maGcnElis > 0)
                await localStorage.SetItemAsync(MaGcnElisKey, maGcnElis);
            if (!string.IsNullOrWhiteSpace(maDinhDanh))
                await localStorage.SetItemAsync(MaDinhDanhKey, maDinhDanh);
        }
    }
    
    public async Task<bool> CheckAuthorByMaGcnElis(long maGcnElis)
    {
        var authenticationState = await ((JwtAuthStateProvider)authStateProvider).GetAuthenticationStateAsync();
        if (authenticationState.User.Identity is not ClaimsIdentity claimsIdentity)
            return false;
        if (claimsIdentity.FindFirst(JwtRegisteredClaimNames.Typ)?.Value.ToLower() == "ldap")
            return true;
        if (maGcnElis <= 0)
            return false;
        var maGcn = await localStorage.GetItemAsync<long>(MaGcnElisKey);
        return maGcn == maGcnElis;
    }
    
    public async Task<long> GetMaGcnElis()
    {
        return await localStorage.GetItemAsync<long>(MaGcnElisKey);
    }
    
    public async Task SetMaGcnElis(long maGcnElis)
    {
        await localStorage.SetItemAsync(MaGcnElisKey, maGcnElis);
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