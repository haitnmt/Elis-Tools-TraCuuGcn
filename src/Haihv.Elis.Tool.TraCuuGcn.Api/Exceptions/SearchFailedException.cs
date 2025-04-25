using System.Net;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;

/// <summary>
/// Exception khi xác thực thất bại.
/// </summary>
public class SearchFailedException : TraCuuGcnException
{
    /// <summary>
    /// Khởi tạo một instance mới của <see cref="TraCuuGcnException"/>.
    /// </summary>
    /// <param name="message">Thông báo lỗi.</param>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    public SearchFailedException(string message, Exception? innerException = null)
        : base(message, HttpStatusCode.BadRequest, "SEARCH_FAILED", innerException)
    {
    }
}
/// <summary>
/// Exception khi không tìm thấy GCN.
/// </summary>
public class SearchNotFoundException : TraCuuGcnException
{
    /// <summary>
    /// Khởi tạo một instance mới của <see cref="TraCuuGcnException"/>.
    /// </summary>
    /// <param name="query">Số phát hành Giấy chứng nhận</param>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    public SearchNotFoundException(string query, Exception? innerException = null)
        : base($"Không tìm thấy GCN theo thông tin: {query}.", HttpStatusCode.NotFound, "GCN_NOT_FOUND", innerException)
    {
    }
}