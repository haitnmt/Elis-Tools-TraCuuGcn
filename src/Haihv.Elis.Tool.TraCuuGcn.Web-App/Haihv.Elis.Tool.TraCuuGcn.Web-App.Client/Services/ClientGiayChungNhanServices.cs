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
    
    private const string UriUpdate = "/api/giay-chung-nhan/update";
    private const string UriHasUpdatePermission = "/api/giay-chung-nhan/has-update-permission";
    private const string UriDeleteMaQr = "/api/giay-chung-nhan/delete-ma-qr";
    
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

    /// <summary>
    /// Cập nhật thông tin pháp lý của giấy chứng nhận thông qua API.
    /// </summary>
    /// <param name="phapLyGiayChungNhan">Thông tin pháp lý của giấy chứng nhận cần cập nhật</param>
    /// <returns>Tuple (bool, string?) trong đó:
    /// - bool: true nếu cập nhật thành công, false nếu thất bại
    /// - string?: thông báo từ API hoặc null
    /// </returns>
    public async Task<(bool success, string? message)> UpdateGiayChungNhanAsync(PhapLyGiayChungNhan phapLyGiayChungNhan)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(UriUpdate, phapLyGiayChungNhan);
            var message = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, message);
        }
        catch (Exception)
        {
            return (false, "Có lỗi xảy ra khi cập nhật thông tin giấy chứng nhận");
        }
    }

    /// <summary>
    /// Kiểm tra quyền cập nhật Giấy chứng nhận dựa vào số serial
    /// </summary>
    /// <param name="serial">Số serial của Giấy chứng nhận cần kiểm tra</param>
    /// <returns>Tuple (bool, string?) trong đó:
    /// - bool: true nếu có quyền cập nhật, false nếu không có quyền
    /// - string?: thông báo từ API hoặc null
    /// </returns>
    public async Task<(bool success, string? message)> GetHasUpdatePermissionAsync(string serial)
    {
        try
        {
            var requestUri = $"{UriHasUpdatePermission}?serial={serial}";
            var response = await httpClient.GetAsync(requestUri);
            var message = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, message);
        }
        catch (Exception)
        {
            return (false, "Lỗi khi kiểm tra quyền cập nhật");
        }
    }

    /// <summary>
    /// Xóa mã QR liên quan đến giấy chứng nhận theo số serial từ API.
    /// </summary>
    /// <param name="serial">Số serial của mã QR cần xóa.</param>
    /// <returns>Tuple (bool, string?) trong đó:
    /// - bool: true nếu xóa thành công, false nếu thất bại
    /// - string?: thông báo từ API hoặc null
    /// </returns>
    public async Task<(bool success, string? message)> DeleteMaQrAsync(string serial)
    {
        var response = await httpClient.DeleteAsync($"{UriDeleteMaQr}?serial={serial}");
        var message = await response.Content.ReadAsStringAsync();
        return (response.IsSuccessStatusCode, message);
    }
}