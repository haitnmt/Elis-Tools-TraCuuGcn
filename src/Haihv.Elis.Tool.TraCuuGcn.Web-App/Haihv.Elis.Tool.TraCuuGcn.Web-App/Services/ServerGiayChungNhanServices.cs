using Haihv.Elis.Tool.TraCuuGcn.Models;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

/// <summary>
/// Lớp triển khai dịch vụ Giấy chứng nhận ở phía server
/// </summary>
/// <remarks>
/// Lớp này thực hiện các cuộc gọi API đến backend để lấy thông tin về Giấy chứng nhận
/// và được sử dụng trong môi trường server-side Blazor
/// </remarks>
/// <param name="httpClient">HttpClient dùng để gửi request đến API</param>
/// <param name="httpContextAccessor">Đối tượng truy cập HttpContext hiện tại</param>
internal class ServerGiayChungNhanServices(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : 
    ServerServices(httpClient, httpContextAccessor), IGiayChungNhanServices
{
    private readonly HttpClient _httpClient = httpClient;

    /// <summary>
    /// URI endpoint để tìm kiếm thông tin Giấy chứng nhận từ API
    /// </summary>
    private const string UriSearch = "/api/search";
    private const string UriDeleteMaQr = "/api/giay-chung-nhan/delete-ma-qr";
    private const string UriCheckPermission = "/api/giay-chung-nhan/has-update-permission";
    private const string UriUpdateGcn = "/api/giay-chung-nhan/update";

    /// <summary>
    /// Xóa mã QR của giấy chứng nhận từ API server.
    /// </summary>
    /// <param name="maGcn">Mã giấy chứng nhận cần xóa mã QR.</param>
    /// <returns>Bộ giá trị gồm cờ trạng thái thành công
    /// và thông báo từ server, hoặc null nếu không có thông báo.</returns>
    public async Task<(bool success, string? message)> DeleteMaQrAsync(string maGcn)
    {
        var uri = $"{UriDeleteMaQr}?serial={maGcn}";
        // Khởi tạo HttpRequestMessage
        using var requestMessage = await CreateHttpRequestMessage(HttpMethod.Delete, uri);

        // Gửi yêu cầu và nhận phản hồi
        using var response = await _httpClient.SendAsync(requestMessage);
        var message = await response.Content.ReadAsStringAsync();
        return (response.IsSuccessStatusCode, message);
    }

    /// <summary>
    /// Kiểm tra quyền cập nhật của mã giấy chứng nhận từ API server.
    /// </summary>
    /// <param name="maGcn">Mã giấy chứng nhận cần kiểm tra quyền cập nhật.</param>
    /// <returns>Bộ giá trị gồm cờ trạng thái thành công
    /// và thông báo từ server, hoặc null nếu không có thông báo.</returns>
    public async Task<(bool success, string? message)> GetHasUpdatePermissionAsync(string maGcn)
    {
        var uri = $"{UriCheckPermission}?serial={maGcn}";
        // Khởi tạo HttpRequestMessage
        using var requestMessage = await CreateHttpRequestMessage(HttpMethod.Get, uri);

        // Gửi yêu cầu và nhận phản hồi
        using var response = await _httpClient.SendAsync(requestMessage);
        var message = await response.Content.ReadAsStringAsync();
        return (response.IsSuccessStatusCode, message);
    }

    /// <summary>
    /// Cập nhật thông tin giấy chứng nhận lên API server.
    /// </summary>
    /// <param name="giayChungNhan">Thông tin giấy chứng nhận cần cập nhật.</param>
    /// <returns>Bộ giá trị gồm cờ trạng thái thành công
    /// và thông báo từ server, hoặc null nếu không có thông báo.</returns>
    public async Task<(bool success, string? message)> UpdateGiayChungNhanAsync(PhapLyGiayChungNhan giayChungNhan)
    {
        // Khởi tạo HttpRequestMessage
        using var requestMessage = await CreateHttpRequestMessage(HttpMethod.Post, UriUpdateGcn);
        requestMessage.Content = JsonContent.Create(giayChungNhan);
        // Gửi yêu cầu và nhận phản hồi
        using var response = await _httpClient.SendAsync(requestMessage);
        var message = await response.Content.ReadAsStringAsync();
        return (response.IsSuccessStatusCode, message);
    }
    
    /// <summary>
    /// Lấy thông tin Giấy chứng nhận theo chuỗi truy vấn từ API server
    /// </summary>
    /// <param name="query">Chuỗi truy vấn tìm kiếm (số serial, mã QR, hoặc mã giấy chứng nhận)</param>
    /// <returns>Đối tượng GiayChungNhanInfo chứa thông tin giấy chứng nhận hoặc null nếu không tìm thấy</returns>
    public async Task<GiayChungNhanInfo?> GetGiayChungNhanInfoAsync(string query)
    {
        var uri = $"{UriSearch}?query={query}";
        return await GetDataAsync<GiayChungNhanInfo>(uri);
    }
}