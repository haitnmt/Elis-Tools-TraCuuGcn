using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

namespace Haihv.Elis.Tool.TraCuuGcn.Web_App.Client.Services;

public class ClientCacheService(HttpClient httpClient) : ICacheService
{
    private const string UriDeleteCache = "cache/delete";

    /// <summary>
    /// Xóa mục nhớ cache tương ứng với số serial được chỉ định.
    /// </summary>
    /// <param name="serial">Số serial của mục nhớ cache cần xóa.</param>
    /// <returns>
    /// Một tuple trong đó phần tử đầu tiên là giá trị boolean cho biết việc xóa có thành công hay không,
    /// và phần tử thứ hai là thông báo lỗi cung cấp thông tin bổ sung nếu thao tác thất bại.
    /// </returns>
    public async Task<(bool success, string? message)> DeleteCacheAsync(string serial)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"{UriDeleteCache}/{serial}");
            var errorMessage = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, errorMessage);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}