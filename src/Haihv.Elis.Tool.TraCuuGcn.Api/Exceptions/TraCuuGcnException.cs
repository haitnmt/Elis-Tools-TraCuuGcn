using System.Net;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;

public abstract class TraCuuGcnException : Exception
{
    /// <summary>
    /// Mã HTTP trả về cho client.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Mã lỗi nội bộ.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Khởi tạo một instance mới của <see cref="TraCuuGcnException"/>.
    /// </summary>
    /// <param name="message">Thông báo lỗi.</param>
    /// <param name="statusCode">Mã HTTP trả về cho client.</param>
    /// <param name="errorCode">Mã lỗi nội bộ.</param>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    protected TraCuuGcnException(string message, HttpStatusCode statusCode, string errorCode, Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }
}