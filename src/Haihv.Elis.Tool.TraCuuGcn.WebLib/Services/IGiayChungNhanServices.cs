using Haihv.Elis.Tool.TraCuuGcn.Models;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;


/// <summary>
/// Interface định nghĩa các dịch vụ liên quan đến Giấy chứng nhận
/// </summary>
/// <remarks>
/// Interface này được sử dụng để truy vấn thông tin Giấy chứng nhận từ server
/// và cung cấp các phương thức cho các tính năng liên quan đến giấy chứng nhận
/// </remarks>
public interface IGiayChungNhanServices
{
    /// <summary>
    /// Lấy thông tin Giấy chứng nhận theo chuỗi truy vấn
    /// </summary>
    /// <param name="query">Chuỗi truy vấn tìm kiếm (số serial, mã QR, hoặc mã giấy chứng nhận)</param>
    /// <returns>Đối tượng GiayChungNhanInfo chứa thông tin giấy chứng nhận hoặc null nếu không tìm thấy</returns>
    Task<GiayChungNhanInfo?> GetGiayChungNhanInfoAsync(string query);
}