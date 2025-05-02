using Haihv.Elis.Tool.TraCuuGcn.Models;
using LanguageExt.Common;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public interface IChuSuDungService
{

    /// <summary>
    /// Lấy thông tin chủ sử dụng và quan hệ chủ sử dụng.
    /// </summary>
    /// <param name="serial"> Số Serial của Giấy chứng nhận.</param>
    /// <param name="cancellationToken">Token hủy bỏ.</param>
    /// <returns>Kết quả chứa thông tin chủ sử dụng hoặc lỗi.</returns>
    Task<Result<List<ChuSuDungInfo>>> GetResultAsync(string? serial = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy thông tin chủ sử dụng theo Mã GCN.
    /// </summary>
    /// <param name="serial"> Số Serial của Giấy chứng nhận.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<ChuSuDungElis>> GetAsync(string? serial = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Lấy thông tin chủ sử dụng theo Mã GCN.
    /// </summary>
    /// <param name="serial"> Số Serial của Giấy chứng nhận.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<ChuSuDungElis>> GetInDatabaseAsync(string? serial = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Lấy Danh sách mã chủ sử dụng.
    /// </summary>
    /// <param name="serial">Số Serial của Giấy chứng nhận.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<long>> GetMaChuSuDungAsync(string? serial = null, CancellationToken cancellationToken = default);
}