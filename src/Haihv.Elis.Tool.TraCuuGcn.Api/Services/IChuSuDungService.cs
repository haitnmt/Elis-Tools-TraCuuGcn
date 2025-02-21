using Haihv.Elis.Tool.TraCuuGcn.Models;
using LanguageExt.Common;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public interface IChuSuDungService
{
    /// <summary>
    /// Lấy thông tin chủ sử dụng theo số định danh.
    /// </summary>
    /// <param name="maGcn">Mã GCN của Giấy chứng nhận.</param>
    /// <param name="soDinhDanh">Số định danh.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Kết quả chứa thông tin xác thực chủ sử dụng hoặc lỗi.</returns>
    Task<Result<AuthChuSuDung>> GetResultAuthChuSuDungAsync(long maGcn = 0, string? soDinhDanh = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy thông tin chủ sử dụng và quan hệ chủ sử dụng.
    /// </summary>
    /// <param name="maGcn">Mã GCN của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Kết quả chứa thông tin chủ sử dụng hoặc lỗi.</returns>
    Task<Result<List<ChuSuDungInfo>>> GetResultAsync(
        long maGcn = 0,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy thông tin chủ sử dụng theo Mã GCN.
    /// </summary>
    /// <param name="maGcnElis"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<ChuSuDungInfo>> GetAsync(
        long maGcnElis = 0, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Lưu thông tin chủ sử dụng vào cache.
    /// </summary>
    /// <param name="maGcnElis">
    /// Mã của Giấy chứng nhận trong hệ thống ELIS.
    /// </param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Thông tin chủ sử dụng hoặc null nếu không tìm thấy.</returns>
    Task SetCacheAuthChuSuDungAsync(long maGcnElis, CancellationToken cancellationToken = default);
}