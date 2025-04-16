using System.Net;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;

/// <summary>
/// Exception khi không tìm thấy GCN.
/// </summary>
public class GiayChungNhanNotFoundException : TraCuuGcnException
{
    /// <summary>
    /// Khởi tạo một instance mới của <see cref="GiayChungNhanNotFoundException"/>.
    /// </summary>
    /// <param name="serial">Số phát hành Giấy chứng nhận</param>
    /// <param name="innerException">Exception gốc (nếu có).</param>
    public GiayChungNhanNotFoundException(string serial, Exception? innerException = null)
        : base($"Không tìm thấy GCN với số phát hành: {serial}.", HttpStatusCode.NotFound, "GCN_NOT_FOUND", innerException)
    {
    }
}