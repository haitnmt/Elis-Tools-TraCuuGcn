using System.Net;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;

/// <summary>
/// Exception khi không tìm thấy thông tin về thửa đất.
/// </summary>
public class ThuaDatNotFoundException : TraCuuGcnException
{
    /// <summary>
    /// Khởi tạo một instance mới của <see cref="TraCuuGcnException"/>.
    /// </summary>
    /// <param name="serial">Số phát hành của Giấy chứng nhận.</param>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    public ThuaDatNotFoundException(string serial, Exception? innerException = null)
        : base($"Không tìm thấy thửa đất với số phát hành Giấy chứng nhận: {serial}.", HttpStatusCode.NotFound, "THUA_DAT_NOT_FOUND",
            innerException)
    {
    }
}