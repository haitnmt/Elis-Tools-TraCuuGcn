using System.Net;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
/// <summary>
/// Exception khi không tìm thấy kết nối cơ sở dữ liệu ELIS
/// </summary>
public class NoConnectionDataElisException : TraCuuGcnException
{
    /// <summary>
    /// Khởi tạo một instance mới của <see cref="TraCuuGcnException"/>.
    /// </summary>
    /// <param name="serial">Số phát hành Giấy chứng nhận.</param>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    public NoConnectionDataElisException(string serial, Exception? innerException = null) : 
        base($"Không tìm thấy kết nối dữ liệu cho số phát hành: {serial}.", HttpStatusCode.NotFound, "NO_CONNECTION_DATA", innerException)
    {
    }
}

/// <summary>
/// Exception khi truy cập cơ sở dữ liệu ELIS
/// </summary>
public class DataElisException : TraCuuGcnException
{
    /// <summary>
    /// Khởi tạo một instance mới của <see cref="TraCuuGcnException"/>.
    /// </summary>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    public DataElisException(Exception? innerException = null) : 
        base($"Lỗi khi truy cập Cơ sở dữ liệu ELIS {innerException?.Message}.", HttpStatusCode.BadRequest, "ERROR_DATA", innerException)
    {
    }
}