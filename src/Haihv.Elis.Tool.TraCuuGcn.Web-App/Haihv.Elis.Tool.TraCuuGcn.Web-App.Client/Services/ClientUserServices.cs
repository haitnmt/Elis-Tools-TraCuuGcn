using System.Net.Http.Json;
using Haihv.Elis.Tool.TraCuuGcn.Models;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

/// <summary>
/// Lớp triển khai dịch vụ người dùng ở phía client
/// </summary>
/// <remarks>
/// Lớp này thực hiện các cuộc gọi API để lấy thông tin người dùng
/// và được sử dụng trong môi trường WebAssembly Blazor
/// </remarks>
/// <param name="httpClient">HttpClient dùng để gửi request đến API</param>
internal sealed class ClientUserServices(HttpClient httpClient) : IUserServices
{
    /// <summary>
    /// URI endpoint để lấy thông tin người dùng từ API authentication
    /// </summary>
    private const string UrlGetUserInfo = "authentication/user/info";

    /// <summary>
    /// Lấy thông tin người dùng hiện tại từ API
    /// </summary>
    /// <returns>Đối tượng UserInfo chứa thông tin người dùng hoặc null nếu không có dữ liệu</returns>
    public async Task<UserInfo?> GetUserInfoAsync()
    {
        return await httpClient.GetFromJsonAsync<UserInfo>(UrlGetUserInfo);
    }
}