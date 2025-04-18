using System.Net;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;

/// <summary>
/// Exception khi không tìm thấy GCN.
/// </summary>
public class GiayChungNhanNotFoundException : TraCuuGcnException
{
    /// <summary>
    /// Khởi tạo một instance mới của <see cref="TraCuuGcnException"/>.
    /// </summary>
    /// <param name="serial">Số phát hành Giấy chứng nhận.</param>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    public GiayChungNhanNotFoundException(string serial, Exception? innerException = null)
        : base($"Không tìm thấy GCN với số phát hành: {serial}.", HttpStatusCode.NotFound, "GCN_NOT_FOUND", innerException)
    {
    }

    /// <summary>
    /// Khởi tạo một instance mới của <see cref="TraCuuGcnException"/>.
    /// </summary>
    /// <param name="maVach">Mã vạch của Giấy chứng nhận.</param>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    public GiayChungNhanNotFoundException(long maVach,Exception? innerException = null)
        : base($"Không tìm thấy GCN với mã vạch: {maVach}.", HttpStatusCode.NotFound, "GCN_NOT_FOUND", innerException)
    {
    }
}

/// <summary>
/// Exception khi tham số Serial truyền vào trống hoặc null.
/// </summary>
public class NoUpdateGiayChungNhanException : TraCuuGcnException
{
    /// <summary>
    /// Khởi tạo một instance mới của <see cref="TraCuuGcnException"/>.
    /// <param name="serial">Số phát hành Giấy chứng nhận.</param>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    /// </summary>
    public NoUpdateGiayChungNhanException(string serial, Exception? innerException = null)
        : base($"Giấy chứng nhận đã phê duyệt, không được cập nhật! Serial: {serial}", HttpStatusCode.Locked, "GCN_APPROVED", innerException)
    {
    }
}

/// <summary>
/// Exception khi Giấy chứng nhận đã được phê duyệt (đã ký) không được cập nhật
/// </summary>
public class NoSerialException : TraCuuGcnException
{
    /// <summary>
    /// Khởi tạo một instance mới của <see cref="TraCuuGcnException"/>.
    /// </summary>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    public NoSerialException(Exception? innerException = null)
        : base($"Số Serial không được để trống.", HttpStatusCode.NotFound, "SERIAL_NULL", innerException)
    {
    }
}