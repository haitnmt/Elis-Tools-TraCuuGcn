using System.Net;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;

/// <summary>
/// Exception khi không tìm thấy thông tin về tài sản
/// </summary>
public class TaiSanNotFoundException : TraCuuGcnException
{
    /// <summary>
    /// Khởi tạo một instance mới của <see cref="TraCuuGcnException"/>.
    /// </summary>
    /// <param name="serial">Số phát hành của Giấy chứng nhận.</param>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    public TaiSanNotFoundException(string serial, Exception? innerException = null)
        : base($"Không tìm thấy tài sản với số phát hành Giấy chứng nhận: {serial}.", HttpStatusCode.NotFound, "TAI_SAN_NOT_FOUND",
            innerException)
    {
    }
}