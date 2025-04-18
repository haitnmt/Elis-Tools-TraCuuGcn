using Haihv.Elis.Tool.TraCuuGcn.Models;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

/// <summary>
/// Interface định nghĩa các dịch vụ liên quan đến người dùng
/// </summary>
/// <remarks>
/// Interface này được sử dụng để truy vấn thông tin người dùng từ server
/// và cung cấp các phương thức cho các tính năng người dùng khác
/// </remarks>
public interface IUserServices
{
    /// <summary>
    /// Lấy thông tin người dùng hiện tại đã đăng nhập
    /// </summary>
    /// <returns>Đối tượng UserInfo chứa thông tin người dùng hoặc null nếu không tìm thấy</returns>
    Task<UserInfo?> GetUserInfoAsync();
}