using Haihv.Elis.Tool.TraCuuGcn.Models;
using LanguageExt.Common;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public interface IGiayChungNhanService
{
    /// <summary>
    /// Lấy thông tin Giấy chứng nhận theo số serial.
    /// </summary>
    /// <param name="serial">Số serial của Giấy chứng nhận.</param>
    /// <param name="maVach">Mã vạch của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ không bắt buộc.</param>
    /// <returns>Kết quả chứa danh sách thông tin Giấy chứng nhận hoặc lỗi nếu không tìm thấy.</returns>
    Task<Result<GiayChungNhan>> GetResultAsync(string? serial = null, long maVach = 0,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy thông tin Giấy chứng nhận theo số serial.
    /// </summary>
    /// <param name="serial">Số serial của Giấy chứng nhận.</param>
    /// <param name="maVach">Mã vạch của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ không bắt buộc.</param>
    /// <returns>Kết quả chứa danh sách thông tin Giấy chứng nhận</returns>
    /// <exception cref="Exception">Ném ra ngoại lệ nếu có lỗi xảy ra trong quá trình truy vấn cơ sở dữ liệu.</exception>
    Task<GiayChungNhan?> GetAsync(string? serial = null, long maVach = 0,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật thông tin Giấy chứng nhận.
    /// </summary>
    /// <param name="phapLyGiayChungNhan">Thông tin Giấy chứng nhận cần cập nhật.</param>
    /// <param name="cancellationToken">Token hủy bỏ tác vụ không bắt buộc.</param>
    /// <returns>Kết quả cập nhật thông tin Giấy chứng nhận.</returns>
    Task<Result<bool>> UpdateAsync(PhapLyGiayChungNhan? phapLyGiayChungNhan, CancellationToken cancellationToken = default);
}