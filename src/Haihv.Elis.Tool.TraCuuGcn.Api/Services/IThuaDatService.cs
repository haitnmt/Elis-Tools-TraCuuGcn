using Haihv.Elis.Tool.TraCuuGcn.Models;
using LanguageExt.Common;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public interface IThuaDatService
{
    /// <summary>
    /// Lấy thông tin Thửa đất theo Serial của Giấy chứng nhận.
    /// </summary>
    /// <param name="serial">Serial của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ không bắt buộc.</param>
    /// <returns>Kết quả chứa thông tin Thửa đất hoặc lỗi nếu không tìm thấy.</returns>
    Task<Result<List<ThuaDat>>> GetResultAsync(string? serial,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Lấy thông tin Thửa đất theo Serial của Giấy chứng nhậ.
    /// </summary>
    /// <param name="serial">Serial của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ không bắt buộc.</param>
    /// <returns>Kết quả chứa thông tin Thửa đất hoặc lỗi nếu không tìm thấy.</returns>
    Task<List<ThuaDat>> GetAsync(string? serial, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách mã Thửa đất theo Serial của Giấy chứng nhận.
    /// </summary>
    /// <param name="serial">Serial của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ không bắt buộc.</param>
    /// <returns>Danh sách mã Thửa đất hoặc null nếu không tìm thấy.</returns>
    Task<List<long>> GetMaThuaDatAsync(string serial, CancellationToken cancellationToken = default);
}