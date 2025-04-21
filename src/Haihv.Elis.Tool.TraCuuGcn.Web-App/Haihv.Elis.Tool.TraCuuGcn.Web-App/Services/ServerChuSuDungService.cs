using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Haihv.Elis.Tool.TraCuuGcn.WebApp.Extensions;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

internal class ServerChuSuDungService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) :
    ServerServices(httpClient, httpContextAccessor), IChuSuDungServices
{
    private readonly HttpClient _httpClient = httpClient;
    public async Task<(List<ChuSuDungInfo> dsChuSuDung, string? message)> GetChuSuDungInfoAsync(string serial, string? soDinhDanh = null)
    {
        try
        {
            // Khởi tạo HttpRequestMessage
            using var requestMessage = await CreateHttpRequestMessage(HttpMethod.Delete, ChuSuDungUri.GetChuSuDungWithQuery(serial, soDinhDanh));
            // Gửi yêu cầu và nhận phản hồi
            using var response = await _httpClient.SendAsync(requestMessage);
            return response.IsSuccessStatusCode ?
                (await response.Content.ReadFromJsonAsync<List<ChuSuDungInfo>>() ?? [], "Không tìm thấy thông tin Chủ sử dụng!") :
                ([], await response.ParseErrorMessageAsync());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ([], e.Message);
        }
    }
}