using System.Net.Http.Json;
using Haihv.Elis.Tool.TraCuuGcn.Models;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

/// <summary>
/// Lớp triển khai dịch vụ Giấy chứng nhận ở phía client
/// </summary>
/// <remarks>
/// Lớp này thực hiện các cuộc gọi API để lấy thông tin Giấy chứng nhận
/// và được sử dụng trong môi trường WebAssembly Blazor
/// </remarks>
/// <param name="httpClient">HttpClient dùng để gửi request đến API</param>
internal sealed class ClientGiayChungNhanServices(HttpClient httpClient) : IGiayChungNhanServices
{
    /// <summary>
    /// URI endpoint để tìm kiếm thông tin Giấy chứng nhận từ API
    /// </summary>
    private const string UriSearch = "/api/search";
    
    /// <summary>
    /// Lấy thông tin Giấy chứng nhận theo chuỗi truy vấn từ API
    /// </summary>
    /// <param name="query">Chuỗi truy vấn tìm kiếm (số serial, mã QR, hoặc mã giấy chứng nhận)</param>
    /// <returns>Đối tượng GiayChungNhanInfo chứa thông tin giấy chứng nhận hoặc null nếu không tìm thấy</returns>
    public async Task<GiayChungNhanInfo?> GetGiayChungNhanInfoAsync(string query)
    {
        var requestUri = $"{UriSearch}?query={query}";
        return await httpClient.GetFromJsonAsync<GiayChungNhanInfo>(requestUri);
    }
}