﻿using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Haihv.Elis.Tool.TraCuuGcn.WebApp.Extensions;

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
    /// Xóa mã QR của giấy chứng nhận từ API server.
    /// </summary>
    /// <param name="serial">Số serial của Giấy chứng nhận</param>
    /// <returns>Bộ giá trị gồm cờ trạng thái thành công
    /// và thông báo từ server, hoặc null nếu không có thông báo.</returns>
    public async Task<(bool success, string? error)> DeleteMaQrAsync(string serial)
    {
        // Khởi tạo HttpRequestMessage
        using var requestMessage = await CreateHttpRequestMessage(HttpMethod.Delete, GiayChungNhanUri.DeleteMaQrWithQuery(serial));

        // Gửi yêu cầu và nhận phản hồi
        using var response = await _httpClient.SendAsync(requestMessage);
        return (response.IsSuccessStatusCode, await response.ParseErrorMessageAsync());
    }

    /// <summary>
    /// Kiểm tra quyền cập nhật của mã giấy chứng nhận từ API server.
    /// </summary>
    /// <param name="serial">Số serial của Giấy chứng nhận</param>
    /// <returns>Bộ giá trị gồm cờ trạng thái thành công
    /// và thông báo từ server, hoặc null nếu không có thông báo.</returns>
    public async Task<(bool success, string? error)> GetHasUpdatePermissionAsync(string serial)
    {
        // Khởi tạo HttpRequestMessage
        using var requestMessage = await CreateHttpRequestMessage(HttpMethod.Get, GiayChungNhanUri.GetHasUpdatePermissionWithQuery(serial));

        // Gửi yêu cầu và nhận phản hồi
        using var response = await _httpClient.SendAsync(requestMessage);
        return (response.IsSuccessStatusCode, await response.ParseErrorMessageAsync());
    }

    /// <summary>
    /// Cập nhật thông tin giấy chứng nhận lên API server.
    /// </summary>
    /// <param name="giayChungNhan">Thông tin giấy chứng nhận cần cập nhật.</param>
    /// <returns>Bộ giá trị gồm cờ trạng thái thành công
    /// và thông báo từ server, hoặc null nếu không có thông báo.</returns>
    public async Task<(bool success, string? error)> UpdateGiayChungNhanAsync(PhapLyGiayChungNhan giayChungNhan)
    {
        // Khởi tạo HttpRequestMessage
        using var requestMessage = await CreateHttpRequestMessage(HttpMethod.Post, GiayChungNhanUri.UpdateGiayChungNhan);
        requestMessage.Content = JsonContent.Create(giayChungNhan);
        // Gửi yêu cầu và nhận phản hồi
        using var response = await _httpClient.SendAsync(requestMessage);
        return (response.IsSuccessStatusCode, await response.ParseErrorMessageAsync());
    }
    
    /// <summary>
    /// Lấy thông tin Giấy chứng nhận theo chuỗi truy vấn từ API server
    /// </summary>
    /// <param name="query">Chuỗi truy vấn tìm kiếm (số serial, mã QR, hoặc mã giấy chứng nhận)</param>
    /// <returns>Đối tượng GiayChungNhanInfo chứa thông tin giấy chứng nhận hoặc null nếu không tìm thấy</returns>
    public async Task<GiayChungNhanInfo?> GetGiayChungNhanInfoAsync(string query)
    {
        return await GetDataAsync<GiayChungNhanInfo>(GiayChungNhanUri.SearchWithQuery(query));
    }
}