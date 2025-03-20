using System.Net;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public class JwtAuthorizationHandler(ILocalStorageService localStorage) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await localStorage.GetItemAsync<string>("authToken", cancellationToken);
        
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        // Xử lý 401 Unauthorized
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            // TODO: Thêm logic refresh token ở đây
        }

        return response;
    }
}