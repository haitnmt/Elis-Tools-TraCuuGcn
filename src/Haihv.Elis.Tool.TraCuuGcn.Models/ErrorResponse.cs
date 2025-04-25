namespace Haihv.Elis.Tool.TraCuuGcn.Models;

/// <summary>
/// Mô hình thông báo lỗi từ API
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Thông tin lỗi chi tiết
    /// </summary>
    public ErrorDetail? Error { get; set; }

    /// <summary>
    /// Lấy thông báo lỗi từ phản hồi
    /// </summary>
    /// <returns>Thông báo lỗi hoặc thông báo mặc định nếu không có</returns>
    public string GetErrorMessage()
    {
        return Error?.Message ?? "Có lỗi xảy ra khi thực hiện yêu cầu";
    }
}

/// <summary>
/// Chi tiết lỗi
/// </summary>
public class ErrorDetail
{
    /// <summary>
    /// Mã lỗi
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Thông báo lỗi
    /// </summary>
    public string? Message { get; set; }
}