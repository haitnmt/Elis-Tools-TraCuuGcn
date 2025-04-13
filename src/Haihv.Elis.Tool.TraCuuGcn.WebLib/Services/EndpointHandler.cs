using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public class EndpointHandler(ILocalStorageService localStorage, IHttpClientFactory httpClientFactory) : DelegatingHandler
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly HttpClient _httpClientAuth = httpClientFactory.CreateClient("AuthEndpoint");
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        
        var token =  await localStorage.GetItemAsync<string>(AuthService.AccessTokenKey, cancellationToken);
        if (string.IsNullOrWhiteSpace(token))
        {
            token = await _httpClientAuth.RefreshToken();
            await localStorage.SetItemAsync(AuthService.AccessTokenKey, token, cancellationToken);
        }
            
        var jwtToken = _tokenHandler.ReadJwtToken(token);
            
        var expiry = jwtToken.ValidTo;
        if (expiry <= DateTime.UtcNow.AddSeconds(60))
        {
            token = await _httpClientAuth.RefreshToken();
            await localStorage.SetItemAsync(AuthService.AccessTokenKey, token, cancellationToken);
        }
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }
}