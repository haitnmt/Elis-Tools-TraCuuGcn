namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

/// <summary>
/// Interface định nghĩa các dịch vụ liên quan đến xử lý bộ nhớ cache
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Xóa cache liên quan đến số serial được cung cấp một cách bất đồng bộ.
    /// </summary>
    /// <param name="serial">Mã định danh duy nhất của cache cần xóa.</param>
    /// <returns>Task đại diện cho hoạt động bất đồng bộ. Kết quả task chứa một tuple cho biết trạng thái thành công và một thông báo tùy chọn cung cấp thông tin bổ sung.</returns>
    Task<(bool success, string? message)> DeleteCacheAsync(string serial);
}