using System.Net.Http.Json;
using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Haihv.Elis.Tool.TraCuuGcn.WebApp.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

namespace Haihv.Elis.Tool.TraCuuGcn.WebApp.Client.Services;

internal class ClientChuSuDungService(HttpClient httpClient) : IChuSuDungServices
{
    public async Task<(List<ChuSuDungInfo> dsChuSuDung, string? message)> GetChuSuDungInfoAsync(string serial, string? soDinhDanh = null)
    {
        try
        {
            var response = await httpClient.GetAsync(ChuSuDungUri.GetChuSuDungWithQuery(serial, soDinhDanh));
            return response.IsSuccessStatusCode ?
                (await response.Content.ReadFromJsonAsync<List<ChuSuDungInfo>>() ?? [], "Không tìm thấy thông tin chủ sử dụng!") :
                ([], await response.ParseErrorMessageAsync());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ([], e.Message);
        }
    }

    public async Task<(bool success, string? error)> GetHasReadPermissionAsync(string serial, string soDinhDanh)
    {
        try
        {
            var response = await httpClient.GetAsync(ChuSuDungUri.GetHasReadPermissionWithQuery(serial, soDinhDanh));
            return (response.IsSuccessStatusCode, await response.ParseErrorMessageAsync());
        }
        catch (Exception)
        {
            return (false, "Lỗi khi kiểm tra quyền truy cập theo số định danh");
        }
    }
}