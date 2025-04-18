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

/// <summary>
/// Exception khi không tìm thấy thông tin về toạ độ thửa đất
/// </summary>
public class ToaDoNotFoundException : TraCuuGcnException
{
    /// <summary>
    /// Khởi tạo một instance mới của <see cref="TraCuuGcnException"/>.
    /// </summary>
    /// <param name="serial">Số phát hành của Giấy chứng nhận.</param>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    public ToaDoNotFoundException(string serial, Exception? innerException = null)
        : base($"Không tìm toạ độ thửa đất với số phát hành Giấy chứng nhận: {serial}.", HttpStatusCode.NotFound, "TOA_DO_THUA_DAT_NOT_FOUND",
            innerException)
    {
    }
}

/// <summary>
/// Exception khi không có thông tin kế nối SDE
/// </summary>
public class SdeNotConfigException : TraCuuGcnException
{
    /// <summary>
    /// Khởi tạo một instance mới của <see cref="TraCuuGcnException"/>.
    /// </summary>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    public SdeNotConfigException(Exception? innerException = null)
        : base("Không có thông tin kết nối API SDE", HttpStatusCode.NotFound, "SDE_NOT_CONFIG",
            innerException)
    {
    }
}

/// <summary>
/// Exception khi có lỗi trong quá trình lấy thông tin toạ độ thửa từ API SDE
/// </summary>
public class SdeGetException : TraCuuGcnException
{
    /// <summary>
    /// Khởi tạo một instance mới của <see cref="TraCuuGcnException"/>.
    /// </summary>
    /// <param name="message">Lỗi trả về từ API SDE</param>
    /// <param name="statusCode">HttpStatusCode trả về từ API SDE</param>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    public SdeGetException(string message, HttpStatusCode statusCode, Exception? innerException = null)
        : base(message, statusCode, "SDE_API_ERROR",
            innerException)
    {
    }
}