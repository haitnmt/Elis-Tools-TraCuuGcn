using Haihv.Elis.Tool.TraCuuGcn.Models;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

/// <summary>
/// Lớp triển khai dịch vụ người dùng ở phía server
/// </summary>
/// <remarks>
/// Lớp này thực hiện các cuộc gọi API đến backend để lấy thông tin người dùng
/// và được sử dụng trong môi trường server-side Blazor
/// </remarks>
/// <param name="httpClient">HttpClient dùng để gửi request đến API</param>
/// <param name="httpContextAccessor">Đối tượng truy cập HttpContext hiện tại</param>
internal sealed class ServerUserServices(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : ServerServices(httpClient, httpContextAccessor), IUserServices
{
    /// <summary>
    /// URL endpoint để lấy thông tin người dùng từ API
    /// </summary>
    private const string UrlGetUserInfo = "api/user/info";

    /// <summary>
    /// Lấy thông tin người dùng hiện tại từ API server
    /// </summary>
    /// <returns>Đối tượng UserInfo chứa thông tin người dùng hoặc null nếu không có dữ liệu</returns>
    public async Task<UserInfo?> GetUserInfoAsync()
    {
        return await GetDataAsync<UserInfo>(UrlGetUserInfo);
    }
}