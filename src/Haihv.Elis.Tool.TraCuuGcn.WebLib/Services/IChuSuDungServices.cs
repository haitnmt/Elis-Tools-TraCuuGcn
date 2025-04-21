using Haihv.Elis.Tool.TraCuuGcn.Models;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

/// <summary>
/// Dịch vụ cung cấp các chức năng liên quan đến Chủ sử dụng
/// </summary>
public interface IChuSuDungServices
{
    /// <summary>
    /// Lấy thông tin chi tiết của Chủ sử dụng theo số Serial Giấy chứng nhận
    /// </summary>
    /// <param name="serial">Số Serial của Giấy chứng nhận cần truy vấn</param>
    /// <param name="soDinhDanh">Số định danh của Chủ sử dụng (CMND/CCCD/Mã số thuế)</param>
    /// <returns>
    /// Tuple chứa danh sách thông tin Chủ sử dụng và thông báo lỗi (nếu có)
    /// </returns>
    /// <remarks>
    /// Phương thức này sẽ trả về tất cả các Chủ sử dụng liên quan đến Giấy chứng nhận có số Serial được cung cấp.
    /// Số định danh cần thiết để xác định quyền truy cập thông tin (Người dùng nội bộ không cần khai báo)
    /// </remarks>
    Task<(List<ChuSuDungInfo> dsChuSuDung, string? message)> GetChuSuDungInfoAsync(string serial, string? soDinhDanh = null);
}