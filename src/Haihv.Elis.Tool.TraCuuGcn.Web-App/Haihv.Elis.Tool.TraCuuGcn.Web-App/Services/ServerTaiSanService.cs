using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Haihv.Elis.Tool.TraCuuGcn.WebApp.Extensions;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

internal class ServerTaiSanService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) :
    ServerServices(httpClient, httpContextAccessor), ITaiSanServices
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<(List<TaiSan> dsTaiSan, string? message)> GetTaiSanAsync(string serial, string? soDinhDanh = null)
    {
        try
        {
            // Khởi tạo HttpRequestMessage
            using var requestMessage = await CreateHttpRequestMessage(HttpMethod.Delete, TaiSanUri.GetTaiSanWithQuery(serial, soDinhDanh));
            // Gửi yêu cầu và nhận phản hồi
            using var response = await _httpClient.SendAsync(requestMessage);
            return response.IsSuccessStatusCode ?
                (await response.Content.ReadFromJsonAsync<List<TaiSan>>() ?? [], "Không tìm thấy thông tin Chủ sử dụng!") :
                ([], await response.ParseErrorMessageAsync());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ([], e.Message);
        }
    }
}