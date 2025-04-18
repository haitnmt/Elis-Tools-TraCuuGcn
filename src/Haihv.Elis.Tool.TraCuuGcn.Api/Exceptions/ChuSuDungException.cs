using System.Net;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;

/// <summary>
/// Exception khi không tìm thấy Chủ sử dụng
/// </summary>
public class ChuSuDungNotFoundException : TraCuuGcnException
{
    /// <summary>
    /// Khởi tạo một instance mới của <see cref="TraCuuGcnException"/>.
    /// </summary>
    /// <param name="serial">Số phát hành Giấy chứng nhận.</param>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    public ChuSuDungNotFoundException(string serial, Exception? innerException = null)
        : base($"Không tìm thấy Chủ sử dụng có Giấy chứng nhận với số phát hành: {serial}.", HttpStatusCode.NotFound, "CSD_NOT_FOUND", innerException)
    {
    }
}
/// <summary>
/// Exception khi có lỗi trong quá trình truy vấn Chủ sử dụng
/// </summary>
public class ChuSuDungSqlException : TraCuuGcnException
{
    /// <summary>
    /// Khởi tạo một instance mới của <see cref="TraCuuGcnException"/>.
    /// </summary>
    /// <param name="serial">Số phát hành Giấy chứng nhận.</param>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    public ChuSuDungSqlException(string serial, Exception? innerException = null)
        : base($"Có lỗi trong quá trình truy vấn chủ sử dụng: {serial}.", HttpStatusCode.NotFound, "CSD_DATA_ERROR", innerException)
    {
    }
}