using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.Cache;

/// <summary>
/// Tính năng xóa cache của Giấy chứng nhận
/// </summary>
/// <remarks>
/// Tính năng này cho phép xóa cache dữ liệu của Giấy chứng nhận và thông tin mã QR
/// dựa trên số Serial được cung cấp
/// </remarks>
public static class DeleteCache
{
    /// <summary>
    /// Lệnh xóa cache thông tin Giấy chứng nhận
    /// </summary>
    /// <param name="Serial">Số Serial của Giấy chứng nhận cần xóa cache</param>
    public record Command(string Serial) : IRequest<bool>;
    
    /// <summary>
    /// Xử lý lệnh xóa cache thông tin Giấy chứng nhận
    /// </summary>
    /// <remarks>
    /// Lớp này thực hiện việc kiểm tra quyền, xóa thông tin mã QR và cache liên quan đến Giấy chứng nhận
    /// </remarks>
    /// <param name="logger">Dịch vụ ghi log</param>
    /// <param name="permissionService">Dịch vụ kiểm tra quyền</param>
    /// <param name="httpContextAccessor">Đối tượng truy cập HttpContext</param>
    /// <param name="gcnQrService">Dịch vụ quản lý mã QR giấy chứng nhận</param>
    public class Handler(
        ILogger logger,
        IPermissionService permissionService,
        IHttpContextAccessor httpContextAccessor,
        IGcnQrService gcnQrService) : IRequestHandler<Command, bool>
    {
        /// <summary>
        /// Xử lý yêu cầu xóa cache thông tin Giấy chứng nhận
        /// </summary>
        /// <param name="request">Lệnh xóa cache chứa số Serial của Giấy chứng nhận</param>
        /// <param name="cancellationToken">Token hủy thao tác</param>
        /// <returns>True nếu xóa cache thành công, ngược lại False</returns>
        /// <exception cref="InvalidOperationException">Ném ra khi HttpContext không khả dụng</exception>
        /// <exception cref="UnauthorizedAccessException">Ném ra khi người dùng không có quyền xóa cache</exception>
        /// <exception cref="NoSerialException">Ném ra khi số Serial không hợp lệ</exception>
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            // Lấy HttpContext, ném ngoại lệ nếu không có
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var serial = request.Serial.ChuanHoa();
            if (string.IsNullOrWhiteSpace(serial))
                throw new NoSerialException();
                
            // Lấy thông tin người dùng
            var user = httpContext.User;
            var email = user.GetEmail();
            
            // Kiểm tra quyền xóa cache, ném ngoại lệ nếu không có quyền
            if (!permissionService.IsLocalUser(user))
                throw new UnauthorizedAccessException("Không có quyền xóa cache cho giấy chứng nhận này");
            
            var url = httpContext.Request.GetDisplayUrl();
            
            // Thực hiện xóa cache thông tin Giấy chứng nhận
            var result = await gcnQrService.DeleteMaQrAsync(serial, cancellationToken);
            
            // Xử lý kết quả xóa cache
            return result.Match(
                succ =>
                {
                    // Ghi log thành công
                    logger.Information("{Email} xóa cache Giấy chứng nhận thành công: {Url}{Serial}",
                        email,
                        url,
                        serial);
                    return succ;
                },
                ex =>
                {
                    // Ghi log lỗi và ném lại ngoại lệ
                    logger.Error(ex, "Lỗi khi xóa cache Giấy chứng nhận: {Url}{Email}{Serial}",
                        url,
                        email,
                        serial);
                    throw ex;
                });
        }
    }
    
    /// <summary>
    /// Cấu hình endpoint cho tính năng xóa cache Giấy chứng nhận
    /// </summary>
    /// <remarks>
    /// Lớp này định nghĩa các endpoint API cho việc xóa cache thông tin Giấy chứng nhận
    /// </remarks>
    public class GiayChungNhanEndpoint : ICarterModule
    {
        /// <summary>
        /// Đăng ký các route cho tính năng xóa cache Giấy chứng nhận
        /// </summary>
        /// <param name="app">Đối tượng cấu hình endpoint</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete(CacheUri.Delete, async (ISender sender, string serial) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Command(serial));
                    return response ? Results.Ok("Xóa cache thành công") : Results.NotFound("Không tìm thấy thông tin cache");
                })
                .RequireAuthorization() // Yêu cầu xác thực
                .WithTags("Cache"); // Gắn tag cho Swagger
        }
    }
}
