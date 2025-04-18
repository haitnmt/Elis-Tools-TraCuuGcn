using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Components.Authorization;
using HttpRequestMessage = System.Net.Http.HttpRequestMessage;

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
    Task<HttpClient> CreateEndpointClient();
}

public class AuthService(
    IHttpClientFactory httpClientFactory,
    ILocalStorageService localStorage,
    AuthenticationStateProvider authStateProvider)
    : IAuthService
{
    private readonly JwtAuthStateProvider _jwtAuthStateProvider = (JwtAuthStateProvider)authStateProvider;  
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Endpoint");
    private readonly HttpClient _httpClientAuth = httpClientFactory.CreateClient("AuthEndpoint");
    private const string SerialKey = "serial";
    private const string MaDinhDanhKey = "maDinhDanh";
    public const string AccessTokenKey = "accessToken";
    public const string AllowRefreshTokenKey = "allowRefreshTokenKey";

    public async Task<AuthResult> LoginByChuSuDung(AuthChuSuDung authChuSuDung)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/elis/auth", authChuSuDung);
            
            var authResponse = await response.Content.ReadFromJsonAsync<Response<LoginToken>>();

            if (authResponse?.Value is null)
                return new AuthResult { Error = authResponse?.ErrorMsg };
                
            await SetLocalStorageAsync(authResponse.Value.AccessToken, authChuSuDung.Serial, authChuSuDung.SoDinhDanh);
            
            // Không cho phép refresh token sau khi đăng nhập bằng chủ sử dụng
            // Vì đăng nhập bằng chủ sử dụng chỉ được dùng một lần duy nhất
            await localStorage.SetItemAsync(AllowRefreshTokenKey, false);
        
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
            Console.WriteLine($"Đang đăng nhập với username: {username}, rememberMe: {rememberMe}");
            
            // Tạo request message để có thể cấu hình thêm
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/login")
            {
                Content = JsonContent.Create(new { username, password, rememberMe })
            };
            
            Console.WriteLine("Đang gửi request đăng nhập...");
            var response = await _httpClientAuth.SendAsync(request);
            
            Console.WriteLine($"Kết quả đăng nhập: {response.StatusCode} - {response.ReasonPhrase}");
            
            if (response.StatusCode != HttpStatusCode.OK)
                return new AuthResult { Error = $"Error: {response.ReasonPhrase}" };
            
            var accessToken = await response.Content.ReadFromJsonAsync<string>();
            if (accessToken is null || string.IsNullOrWhiteSpace(accessToken))
                return new AuthResult { Error = "Đăng nhập không thành công. Vui lòng thử lại!" };
            
            Console.WriteLine("Đăng nhập thành công, đã nhận access token");
                
            // Lưu access token vào local storage
            await SetLocalStorageAsync(accessToken);
            
            // Đánh dấu cho phép refresh token sau khi đăng nhập thành công
            await localStorage.SetItemAsync(AllowRefreshTokenKey, true);

            // Cập nhật trạng thái xác thực
            var authenticationState = await _jwtAuthStateProvider.GetAuthenticationStateAsync();
            _jwtAuthStateProvider.NotifyUserChanged(authenticationState);

            return new AuthResult { Success = true };
        }
        catch (Exception e)
        {
            Console.WriteLine($"Lỗi khi đăng nhập: {e.Message}");
            if (e.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {e.InnerException.Message}");
            }
            return new AuthResult { Error = e.Message };
        }
    }
    
    private async Task SetLocalStorageAsync(string? accessToken = null, string? serial = null, string maDinhDanh = "")
    {
        if (accessToken is not null)
        {
            // Lưu token vào localStorage - điều này sẽ kích hoạt sự kiện storage trên các tab khác
            await localStorage.SetItemAsync(AccessTokenKey, accessToken);
            if (!string.IsNullOrWhiteSpace(serial))
                await localStorage.SetItemAsync(SerialKey, serial);
            if (!string.IsNullOrWhiteSpace(maDinhDanh))
                await localStorage.SetItemAsync(MaDinhDanhKey, maDinhDanh);
        }
    }
    
    public async Task<HttpClient> CreateEndpointClient()
    {
        var accessToken = await GetAccessTokenAsync();
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            Console.WriteLine("Access token không tồn tại trong local storage");
            await Logout();
            return _httpClient;
        }
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return _httpClient;
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
            Console.WriteLine("Đang thực hiện đăng xuất...");
            // Kiểm tra xem có phải người dùng LDAP hay không
            if (await IsLdapUser())
            {
                var token = await localStorage.GetItemAsync<string>(AccessTokenKey);
                // Tạo request message để có thể cấu hình thêm
                var request = new HttpRequestMessage(HttpMethod.Post, "/api/logout");
            
                // Thêm Authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                // Tham số all=false để chỉ đăng xuất thiết bị hiện tại
                request.Content = JsonContent.Create(false);
            
                Console.WriteLine("Đang gửi request đăng xuất...");
                var response = await _httpClientAuth.SendAsync(request);
            
                Console.WriteLine($"Kết quả đăng xuất: {response.StatusCode} - {response.ReasonPhrase}");
                // Đánh dấu không cho phép refresh token sau khi đăng xuất
                await localStorage.SetItemAsync(AllowRefreshTokenKey, false);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine($"Lỗi khi đăng xuất: {e.Message}");
            if (e.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {e.InnerException.Message}");
            }
        }
        
        // Xóa token khỏi local storage - điều này sẽ kích hoạt sự kiện storage trên các tab khác
        await localStorage.RemoveItemAsync(MaDinhDanhKey);
        await localStorage.RemoveItemAsync(AccessTokenKey);
        
        Console.WriteLine("Đã xóa token khỏi local storage");
        
        // Cập nhật trạng thái xác thực
        _jwtAuthStateProvider.NotifyUserChanged(JwtAuthStateProvider.AnonymousUser);
        
        Console.WriteLine("Đăng xuất hoàn tất");
    }
    
    private async Task<string?> GetAccessTokenAsync()
    {
        try
        {
            var token = await localStorage.GetItemAsync<string>(AccessTokenKey);
            // Kiểm tra xem access token có còn hợp lệ không
            var jwtToken = _tokenHandler.ReadJwtToken(token);
            var expiry = jwtToken.ValidTo;
            // Access token chưa hết hạn thì trả về trực tiếp (nếu còn 60s nữa hết hạn)
            if (expiry > DateTime.UtcNow.AddSeconds(60)) return token; 

            Console.WriteLine("Access token sắp hết hạn, đang thực hiện refresh token...");
            
            // Nếu access token đã hết hạn, thực hiện refresh token

            var accessToken = await _httpClientAuth.RefreshToken(localStorage);

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                // Cập nhật trạng thái xác thực là anonymous
                _jwtAuthStateProvider.NotifyUserChanged(JwtAuthStateProvider.AnonymousUser);
                return null;
            }
            
            // Lưu access token mới vào local storage
            await SetLocalStorageAsync(accessToken);
            
            // Cập nhật trạng thái xác thực
            var authenticationState = await _jwtAuthStateProvider.GetAuthenticationStateAsync();
            _jwtAuthStateProvider.NotifyUserChanged(authenticationState);
            return accessToken;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi refresh token: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
            return null;
        }
    }
    
    
}

public record AuthResult(bool Success = false, string? Error = null);

public static class AuthExtensions
{
    public static async Task<string?> RefreshToken(this HttpClient httpClient, ILocalStorageService localStorage)
    {
        try
        {
            var statusRefreshToken = await localStorage.GetItemAsync<bool?>(AuthService.AllowRefreshTokenKey);
            if (statusRefreshToken is null || !statusRefreshToken.Value)
            {
                Console.WriteLine("Không thực hiện refresh token");
                return null;
            }
            Console.WriteLine("Đang thực hiện refresh token...");
            
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/refreshToken");
            
            var response = await httpClient.SendAsync(request);
            
            Console.WriteLine($"Kết quả refresh token: {response.StatusCode} - {response.ReasonPhrase}");

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine($"Refresh token thất bại: {response.ReasonPhrase}");
                await localStorage.SetItemAsync(AuthService.AllowRefreshTokenKey, false);
                return null;
            }
            
            var accessToken = await response.Content.ReadFromJsonAsync<string>();
            if (accessToken is null || string.IsNullOrWhiteSpace(accessToken))
            {
                Console.WriteLine("Không nhận được access token từ server");
                await localStorage.SetItemAsync(AuthService.AllowRefreshTokenKey, false);
                return null;
            }
            
            Console.WriteLine("Refresh token thành công, đã nhận access token mới");
            await localStorage.SetItemAsync(AuthService.AllowRefreshTokenKey, true);

            return accessToken;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Lỗi khi refresh token: {e.Message}");
            if (e.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {e.InnerException.Message}");
            }
            await localStorage.SetItemAsync(AuthService.AllowRefreshTokenKey, false);
            return null;
        }
    }
}