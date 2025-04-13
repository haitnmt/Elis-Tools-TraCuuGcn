using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public interface IAuthService
{
    Task<AuthResult> LoginByChuSuDung(AuthChuSuDung authChuSuDung);
    Task<AuthResult> LoginUser(string username, string password, bool rememberMe = false);
    Task<bool> CheckAuthorBySerialElis(string? serial);
    Task<string?> GetSerialElis();
    Task SetSerialElis(string? serial);
    Task<bool> IsLdapUser();
    Task Logout();
    Task<HttpClient> CreateHttpClient();
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
    public const string AccessTokenKey = "accessToken";

    public async Task<AuthResult> LoginByChuSuDung(AuthChuSuDung authChuSuDung)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/elis/auth", authChuSuDung);
            
            var authResponse = await response.Content.ReadFromJsonAsync<Response<LoginToken>>();

            if (authResponse?.Value is null)
                return new AuthResult { Error = authResponse?.ErrorMsg };
                
            await SetLocalStorageAsync(authResponse.Value.AccessToken, authChuSuDung.Serial, authChuSuDung.SoDinhDanh);
        
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
    public async Task<AuthResult> LoginUser(string username, string password, bool rememberMe = false)
    {
        try
        {
            // Tạo request message để có thể cấu hình thêm
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/login")
            {
                Content = JsonContent.Create(new { username, password, rememberMe })
            };
            
            // Cấu hình request để gửi và nhận cookie
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            
            var response = await _httpClientAuth.SendAsync(request);
            
            if (response.StatusCode != HttpStatusCode.OK)
                return new AuthResult { Error = $"Error: {response.ReasonPhrase}" };
                
            var accessToken = await response.Content.ReadFromJsonAsync<string>();
            if (accessToken is null || string.IsNullOrWhiteSpace(accessToken))
                return new AuthResult { Error = "Đăng nhập không thành công. Vui lòng thử lại!" };
                
            await SetLocalStorageAsync(accessToken);

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
    
    private async Task SetLocalStorageAsync(string? accessToken = null, string? serial = null, string maDinhDanh = "")
    {
        if (accessToken is not null)
        {
            await localStorage.SetItemAsync(AccessTokenKey, accessToken);
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
    
    public async Task<bool> IsLdapUser()
    {
        var authenticationState = await ((JwtAuthStateProvider)authStateProvider).GetAuthenticationStateAsync();
        if (authenticationState.User.Identity is not ClaimsIdentity claimsIdentity)
            return false;
        return claimsIdentity.FindFirst(JwtRegisteredClaimNames.Typ)?.Value.ToLower() == "ldap";
    }
    
    public async Task<string?> GetSerialElis()
    {
        return await localStorage.GetItemAsync<string>(SerialKey);
    }
    
    public async Task SetSerialElis(string? serial)
    {
        await localStorage.SetItemAsync(SerialKey, serial);
    }
    public async Task Logout()
    {
        try
        {
            var token = await localStorage.GetItemAsync<string>(AccessTokenKey);
            
            // Tạo request message để có thể cấu hình thêm
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/logout");
            
            // Thêm Authorization header
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            // Cấu hình request để gửi cookie
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            
            await _httpClientAuth.SendAsync(request);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        await localStorage.RemoveItemAsync(AccessTokenKey);
        await localStorage.RemoveItemAsync(MaDinhDanhKey);
        _jwtAuthStateProvider.NotifyUserChanged(JwtAuthStateProvider.AnonymousUser);
    }

    private async Task<string?> GetAuthenToken()
    {
        return await localStorage.GetItemAsync<string>(AccessTokenKey);
    }

    public async Task<HttpClient> CreateHttpClient()
    {
        var authenticationState = await ((JwtAuthStateProvider)authStateProvider).GetAuthenticationStateAsync();
        if (authenticationState.User.Identity is not ClaimsIdentity claimsIdentity)
            return _httpClient;
        var token = await GetAuthenToken();
        if (claimsIdentity.IsAuthenticated && !string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            await Logout();
        }
        return _httpClient;
    }
}

public record AuthResult(bool Success = false, string? Error = null);