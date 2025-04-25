using Haihv.Elis.Tool.TraCuuGcn.Models;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

/// <summary>
/// Interface cung cấp các dịch vụ để truy vấn thông tin về thửa đất.
/// </summary>
/// <remarks>
/// Interface này cung cấp các phương thức để truy vấn thông tin công khai, 
/// thông tin đầy đủ và tọa độ của thửa đất dựa trên số serial và số định danh.
/// </remarks>
public interface IThuaDatServices
{
    /// <summary>
    /// Lấy danh sách thông tin công khai của thửa đất dựa trên số serial và số định danh (tùy chọn).
    /// </summary>
    /// <param name="serial">Số serial của thửa đất.</param>
    /// <returns>
    /// Tuple gồm hai phần tử:
    /// - dsThuaDatCongKhai: Danh sách thông tin công khai của thửa đất
    /// - message: Thông báo từ hệ thống hoặc null nếu không có thông báo
    /// </returns>
    /// <remarks>
    /// Phương thức này chỉ trả về các thông tin công khai của thửa đất mà không bao gồm 
    /// các thông tin chi tiết nội bộ.
    /// </remarks>
    Task<(List<ThuaDatPublic> dsThuaDatCongKhai, string? message)> GetThuaDatPublicAsync(string serial);

    /// <summary>
    /// Lấy danh sách thông tin đầy đủ của thửa đất dựa trên số serial và số định danh (tùy chọn).
    /// </summary>
    /// <param name="serial">Số serial của thửa đất.</param>
    /// <param name="soDinhDanh">Số định danh (tùy chọn).</param>
    /// <returns>
    /// Tuple gồm hai phần tử:
    /// - dsThuaDat: Danh sách thông tin đầy đủ của thửa đất
    /// - message: Thông báo từ hệ thống hoặc null nếu không có thông báo
    /// </returns>
    /// <remarks>
    /// Số định danh cần thiết để xác định quyền truy cập thông tin (Người dùng nội bộ không cần khai báo)
    /// Phương thức này trả về thông tin đầy đủ của thửa đất bao gồm 
    /// cả các thông tin chi tiết không công khai.
    /// </remarks>
    Task<(List<ThuaDat> dsThuaDat, string? message)> GetThuaDatAsync(string serial, string? soDinhDanh = null);

    /// <summary>
    /// Lấy thông tin tọa độ của thửa đất dựa trên số serial và số định danh (tùy chọn).
    /// </summary>
    /// <param name="serial">Số serial của thửa đất.</param>
    /// <param name="soDinhDanh">Số định danh (tùy chọn).</param>
    /// <returns>
    /// Tuple gồm hai phần tử:
    /// - toaDoThuaDat: Đối tượng chứa thông tin tọa độ của thửa đất
    /// - message: Thông báo từ hệ thống hoặc null nếu không có thông báo
    /// </returns>
    /// <remarks>
    /// Số định danh cần thiết để xác định quyền truy cập thông tin (Người dùng nội bộ không cần khai báo)
    /// Thông tin tọa độ được trả về dưới dạng object chung để có thể chứa các 
    /// định dạng tọa độ khác nhau (như GeoJSON, WKT, v.v.).
    /// </remarks>
    Task<(object? toaDoThuaDat, string? message)> GetToaDoThuaDatAsync(string serial, string? soDinhDanh = null);
}