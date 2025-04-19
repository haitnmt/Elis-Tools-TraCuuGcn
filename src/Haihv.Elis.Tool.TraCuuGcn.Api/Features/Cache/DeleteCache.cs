using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Hybrid;
using ILogger = Serilog.ILogger;
namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.Cache;

/// <summary>
/// Chức năng xóa cache của giấy chứng nhận theo số serial.
/// </summary>
public static class DeleteCache
{
    /// <summary>
    /// Yêu cầu xóa cache của giấy chứng nhận theo số serial.
    /// </summary>
    /// <param name="Serial">Số serial của giấy chứng nhận.</param>
    public record Query(string Serial) : IRequest<bool>;
    
    /// <summary>
    /// Xử lý yêu cầu xóa cache.
    /// </summary>
    public class Handler(ILogger logger,
        HybridCache hybridCache,
        IHttpContextAccessor httpContextAccessor,
        IPermissionService permissionService) : IRequestHandler<Query, bool>
    {
        /// <summary>
        /// Xử lý yêu cầu xóa cache theo số serial.
        /// </summary>
        /// <param name="request">Yêu cầu chứa số serial của giấy chứng nhận.</param>
        /// <param name="cancellationToken">Token hủy bỏ thao tác.</param>
        /// <returns>True nếu xóa cache thành công, ngược lại là false.</returns>
        /// <exception cref="NoSerialException">Khi số serial không hợp lệ.</exception>
        /// <exception cref="InvalidOperationException">Khi HttpContext không khả dụng.</exception>
        /// <exception cref="UnauthorizedAccessException">Khi người dùng không có quyền xóa cache.</exception>
        public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
        {
            // Chuẩn hóa số serial và kiểm tra tính hợp lệ
            var serial = request.Serial.ChuanHoa();
            if (string.IsNullOrWhiteSpace(serial))
                throw new NoSerialException();
            
            // Kiểm tra HttpContext và quyền người dùng
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var user = httpContext.User;
            
            // Chỉ cho phép người dùng local xóa cache
            if (!permissionService.IsLocalUser(user))
                throw new UnauthorizedAccessException();
            
            // Lấy thông tin URL và email người dùng để ghi log
            var url = httpContext.Request.GetDisplayUrl();
            var email = user.GetEmail();
            
            try
            {
                // Xóa tất cả cache có tag trùng với số serial
                await hybridCache.RemoveByTagAsync(serial, cancellationToken);
                
                // Ghi log thành công
                logger.Information("{Email} xóa cache thành công: {Url} {Serial}",
                    email, url, serial);
                return true;
            }
            catch (Exception e)
            {
                // Ghi log lỗi và trả về false
                logger.Error(e, "Lỗi khi xóa cache: {Url} {Email} {Serial}",
                    url, email, serial);
                return false;
            }
        }
    }
    
    /// <summary>
    /// Định nghĩa endpoint cho việc xóa cache.
    /// </summary>
    public class CacheEndpoint : ICarterModule
    {
        /// <summary>
        /// Cấu hình route cho endpoint xóa cache.
        /// </summary>
        /// <param name="app">Builder để đăng ký endpoint.</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/cache", async (ISender sender, string serial) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(serial));
                    return response ? Results.Ok("Xóa Cache thành công!") : Results.BadRequest("Lỗi khi xóa cache");
                })
                .RequireAuthorization()
                .WithTags("Cache")
                .WithName("DeleteCache")
                .WithDescription("Xóa cache theo số serial của Giấy chứng nhận");
        }
    }
}