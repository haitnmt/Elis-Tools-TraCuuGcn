using System.Net.Http.Json;
using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Haihv.Elis.Tool.TraCuuGcn.WebApp.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

namespace Haihv.Elis.Tool.TraCuuGcn.WebApp.Client.Services;

internal class ClientThuaDatService(HttpClient httpClient) : IThuaDatServices
{
    public async Task<(List<ThuaDatPublic> dsThuaDatCongKhai, string? message)> GetThuaDatPublicAsync(string serial)
    {
        try
        {
            var response = await httpClient.GetAsync(ThuaDatUri.GetThuaDatPublicWithQuery(serial));
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
            var response = await httpClient.GetAsync(ThuaDatUri.GetThuaDatWithQuery(serial, soDinhDanh));
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
            var response = await httpClient.GetAsync(ThuaDatUri.GetToaDoWithQuery(serial, soDinhDanh));
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