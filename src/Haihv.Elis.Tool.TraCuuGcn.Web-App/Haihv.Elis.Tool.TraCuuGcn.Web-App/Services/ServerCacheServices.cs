using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

internal class ServerCacheServices(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : 
    ServerServices(httpClient, httpContextAccessor), ICacheService
{
    private readonly HttpClient _httpClient = httpClient;
    public async Task<(bool success, string? message)> DeleteCacheAsync(string serial)
    {
        // Khởi tạo HttpRequestMessage
        using var requestMessage = await CreateHttpRequestMessage(HttpMethod.Delete, CacheUri.DeleteWithQuery(serial));

        // Gửi yêu cầu và nhận phản hồi
        using var response = await _httpClient.SendAsync(requestMessage);
        var message = await response.Content.ReadAsStringAsync();
        return (response.IsSuccessStatusCode, message);
    }
}