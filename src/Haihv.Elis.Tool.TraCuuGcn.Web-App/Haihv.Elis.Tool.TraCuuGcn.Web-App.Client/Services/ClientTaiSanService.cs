using System.Net.Http.Json;
using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Haihv.Elis.Tool.TraCuuGcn.WebApp.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

namespace Haihv.Elis.Tool.TraCuuGcn.WebApp.Client.Services;

public class ClientTaiSanService(HttpClient httpClient) : ITaiSanServices
{
    public async Task<(List<TaiSan> dsTaiSan, string? message)> GetTaiSanAsync(string serial,
        string? soDinhDanh = null)
    {
        try
        {
            var response = await httpClient.GetAsync(TaiSanUri.GetTaiSanWithQuery(serial, soDinhDanh));
            return response.IsSuccessStatusCode ?
                (await response.Content.ReadFromJsonAsync<List<TaiSan>>() ?? [], "Không tìm thấy thông tin tài sản!") :
                ([], await response.ParseErrorMessageAsync());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ([], e.Message);
        }
    }
}