using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public class JwtAuthStateProvider(ILocalStorageService localStorage, IHttpClientFactory httpClientFactory)
    : AuthenticationStateProvider
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly HttpClient _httpClientAuth = httpClientFactory.CreateClient("AuthEndpoint");

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var jwtToken = await GetOrRefreshJwtSecurityToken();

            if (jwtToken is null)
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

    private async Task<JwtSecurityToken?> GetOrRefreshJwtSecurityToken()
    {
        var token = await localStorage.GetItemAsync<string>(AuthService.AccessTokenKey);
        if (string.IsNullOrWhiteSpace(token)) return null;
        
        try
        {
            // Kiểm tra xem access token có còn hợp lệ không
            var jwtToken = _tokenHandler.ReadJwtToken(token);
            var expiry = jwtToken.ValidTo;
            
            // Access token chưa hết hạn thì trả về trực tiếp (nếu còn 60s nữa hết hạn)
            if (expiry > DateTime.UtcNow.AddSeconds(60)) return jwtToken; 

            Console.WriteLine("Access token sắp hết hạn, đang thực hiện refresh token...");
            
            // Gọi API để lấy lại access token mới (cookie refreshToken sẽ tự động được gửi đi)
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/refreshToken");
            
            // Cấu hình request để gửi cookie
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            
            var response = await _httpClientAuth.SendAsync(request);
            
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine($"Refresh token thất bại: {response.StatusCode} - {response.ReasonPhrase}");
                
                // Kiểm tra nếu là Unauthorized hoặc Forbidden, xóa token cũ
                if (response.StatusCode == HttpStatusCode.Unauthorized || 
                    response.StatusCode == HttpStatusCode.Forbidden)
                {
                    await localStorage.RemoveItemAsync(AuthService.AccessTokenKey);
                }
                
                return null;
            }

            var accessToken = await response.Content.ReadFromJsonAsync<string>();
            
            // Kiểm tra kết quả trả về từ API
            if (accessToken is null || string.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine("Refresh token thất bại: Token trả về rỗng");
                return null;
            }
            
            Console.WriteLine("Refresh token thành công, đã nhận access token mới");
            
            // Lưu lại access token vào local storage
            await SetLocalStorageAsync(accessToken);

            // Trả về JWTSecurityToken mới
            return _tokenHandler.ReadJwtToken(accessToken);
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

    private async Task SetLocalStorageAsync(string? accessToken = null)
    {
        if (accessToken is not null)
        {
            await localStorage.SetItemAsync(AuthService.AccessTokenKey, accessToken);
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