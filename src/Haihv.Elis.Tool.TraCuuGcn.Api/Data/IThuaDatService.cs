using Haihv.Elis.Tool.TraCuuGcn.Models;
using LanguageExt.Common;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Data;

public interface IThuaDatService
{
    /// <summary>
    /// Lấy thông tin Thửa đất theo Serial của Giấy chứng nhận.
    /// </summary>
    /// <param name="maGcn">Mã GCN của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ không bắt buộc.</param>
    /// <returns>Kết quả chứa thông tin Thửa đất hoặc lỗi nếu không tìm thấy.</returns>
    Task<Result<ThuaDat>> GetResultAsync(long maGcn,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy thông tin Thửa đất theo Serial của Giấy chứng nhận từ cơ sở dữ liệu.
    /// </summary>
    /// <param name="maGcn">Mã GCN của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ không bắt buộc.</param>
    /// <returns>Kết quả chứa thông tin Thửa đất hoặc lỗi nếu không tìm thấy.</returns>
    Task<ThuaDat?> GetThuaDatInDatabaseAsync(long maGcn = 0,
        CancellationToken cancellationToken = default);
}