using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Haihv.Elis.Tool.TraCuuGcn.WebApp.Extensions;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

internal class ServerThuaDatService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) :
    ServerServices(httpClient, httpContextAccessor), IThuaDatServices
{
    private readonly HttpClient _httpClient = httpClient;
    public async Task<(List<ThuaDatPublic> dsThuaDatCongKhai, string? message)> GetThuaDatPublicAsync(string serial)
    {
        try
        {
            // Khởi tạo HttpRequestMessage
            using var requestMessage = await CreateHttpRequestMessage(HttpMethod.Delete, ThuaDatUri.GetThuaDatPublicWithQuery(serial));
            // Gửi yêu cầu và nhận phản hồi
            using var response = await _httpClient.SendAsync(requestMessage);
            return response.IsSuccessStatusCode ?
                (await response.Content.ReadFromJsonAsync<List<ThuaDatPublic>>() ?? [], "Không tìm thấy thông tin Thửa đất!") :
                ([], await response.ParseErrorMessageAsync());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ([], e.Message);
        }
    }

    public async Task<(List<ThuaDat> dsThuaDat, string? message)> GetThuaDatAsync(string serial, string? soDinhDanh = null)
    {
        try
        {
            // Khởi tạo HttpRequestMessage
            using var requestMessage = await CreateHttpRequestMessage(HttpMethod.Delete, ThuaDatUri.GetThuaDatWithQuery(serial, soDinhDanh));
            // Gửi yêu cầu và nhận phản hồi
            using var response = await _httpClient.SendAsync(requestMessage);
            return response.IsSuccessStatusCode ?
                (await response.Content.ReadFromJsonAsync<List<ThuaDat>>() ?? [], "Không tìm thấy thông tin Thửa đất!") :
                ([], await response.ParseErrorMessageAsync());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ([], e.Message);
        }
    }

    public async Task<(object? toaDoThuaDat, string? message)> GetToaDoThuaDatAsync(string serial, string? soDinhDanh = null)
    {
        try
        {
            // Khởi tạo HttpRequestMessage
            using var requestMessage = await CreateHttpRequestMessage(HttpMethod.Delete, ThuaDatUri.GetToaDoWithQuery(serial, soDinhDanh));
            // Gửi yêu cầu và nhận phản hồi
            using var response = await _httpClient.SendAsync(requestMessage);
            return response.IsSuccessStatusCode ?
                (await response.Content.ReadFromJsonAsync<object>() ?? null, "Không tìm thấy thông tin tọa độ thửa đất!") :
                (null, await response.ParseErrorMessageAsync());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return (null, e.Message);
        }
    }
}