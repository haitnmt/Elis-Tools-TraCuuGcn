using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Hybrid;
using ILogger = Serilog.ILogger;


namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.GiayChungNhan;

/// <summary>
/// Lớp tĩnh chứa các thành phần liên quan đến việc xóa mã QR của Giấy chứng nhận.
/// </summary>
/// <remarks>
/// Lớp này cung cấp chức năng xóa mã QR của Giấy chứng nhận dựa trên số serial.
/// Quá trình xóa bao gồm việc xóa dữ liệu từ cơ sở dữ liệu và xóa cache liên quan.
/// </remarks>
public static class DeleteMaQr
{
    /// <summary>
    /// Lớp truy vấn để xóa mã QR của Giấy chứng nhận.
    /// </summary>
    /// <param name="Serial">Số serial của Giấy chứng nhận cần xóa mã QR.</param>
    public record Query(string Serial) : IRequest<bool>;

    /// <summary>
    /// Lớp xử lý cho việc xóa mã QR của Giấy chứng nhận.
    /// </summary>
    /// <remarks>
    /// Lớp này xử lý quá trình xóa mã QR, kiểm tra quyền của người dùng,
    /// ghi log và xóa cache liên quan sau khi xóa thành công.
    /// </remarks>
    public class Handler(
        ILogger logger,
        HybridCache hybridCache,
        IPermissionService permissionService,
        IHttpContextAccessor httpContextAccessor,
        ILogElisDataServices logElisDataServices,
        IGcnQrService gcnQrService) : IRequestHandler<Query, bool>
    {
        /// <summary>
        /// Xử lý việc xóa mã QR của Giấy chứng nhận.
        /// </summary>
        /// <param name="request">Yêu cầu chứa số serial của Giấy chứng nhận.</param>
        /// <param name="cancellationToken">Token để hủy bỏ thao tác nếu cần.</param>
        /// <returns>
        /// Trả về true nếu xóa thành công, false nếu xóa thất bại.
        /// </returns>
        /// <exception cref="NoSerialException">Ném ra khi số serial không hợp lệ.</exception>
        /// <exception cref="InvalidOperationException">Ném ra khi HttpContext không khả dụng.</exception>
        /// <exception cref="UnauthorizedAccessException">Ném ra khi người dùng không có quyền xóa mã QR.</exception>
        public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
        {
            // Chuẩn hóa số serial
            var serial = request.Serial.ChuanHoa();
            if (string.IsNullOrWhiteSpace(serial))
                throw new NoSerialException();
                
            // Lấy HttpContext, ném ngoại lệ nếu không có
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
                              
            // Lấy thông tin người dùng
            var user = httpContext.User;
            var email = user.GetEmail();
            
            // Kiểm tra quyền cập nhật, ném ngoại lệ nếu không có quyền
            if (!await permissionService.HasUpdatePermission(user, serial, cancellationToken))
                throw new UnauthorizedAccessException();
            
            var url = httpContext.Request.GetDisplayUrl();
            
            // Thực hiện xóa mã QR
            var result = await gcnQrService.DeleteMaQrAsync(serial, cancellationToken);
            
            // Xử lý kết quả
            return result.Match(
                succ =>
                {
                    if (succ)
                    {
                        // Ghi log thành công
                        logger.Information("{Email} Xóa mã QR thành công: {Url}",
                            email,
                            url);
                            
                        // Ghi log vào ELIS Data
                        logElisDataServices.WriteLogToElisDataAsync(serial, email, url,
                            $"Xóa mã QR của Giấy chứng nhận có phát hành (Serial): {serial}",
                            LogElisDataServices.LoaiTacVu.Xoa, cancellationToken);
                            
                        // Xóa cache liên quan
                        _ = hybridCache.RemoveByTagAsync(serial, cancellationToken).AsTask();
                    }
                    else
                    {                    
                        // Ghi log cảnh báo khi xóa không thành công
                        logger.Warning("{Email} Xóa mã QR không thành công: {Url}",
                            email,
                            url);
                    }
                    return succ;
                },
                ex =>
                {
                    // Ghi log lỗi
                    logger.Error(ex, "{Email} Xóa mã QR không thành công: {Url} {Message}",
                        email,
                        url,
                        ex.Message);
                    return false;
                });
        }
    }

    /// <summary>
    /// Lớp endpoint để định nghĩa API xóa mã QR của Giấy chứng nhận.
    /// </summary>
    public class GiayChungNhanEndpoint : ICarterModule
    {
        /// <summary>
        /// Thêm route DELETE để xóa mã QR của Giấy chứng nhận.
        /// </summary>
        /// <param name="app">Builder để đăng ký endpoint.</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete(GiayChungNhanUri.DeleteMaQr, async (ISender sender, string serial) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(serial));
                    return response ? Results.Ok("Xóa mã QR thành công!") : Results.BadRequest("Lỗi khi xóa mã QR");
                })
                .RequireAuthorization()
                .WithTags("Cache")
                .WithName("DeleteMaQr")
                .WithDescription("Xóa mã QR theo số serial của Giấy chứng nhận")
                .WithSummary("Xóa mã QR của Giấy chứng nhận");
        }
    }
}