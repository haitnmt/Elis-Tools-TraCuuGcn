using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using ILogger = Serilog.ILogger;
using MediatR;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.User;

/// <summary>
/// Cung cấp tính năng lấy thông tin người dùng hiện tại đang đăng nhập.
/// </summary>
public static class GetUserInfo
{
    /// <summary>
    /// Đại diện cho một yêu cầu lấy thông tin người dùng.
    /// </summary>
    public record Query : IRequest<UserInfo>;

    /// <summary>
    /// Xử lý yêu cầu lấy thông tin người dùng hiện tại.
    /// </summary>
    /// <param name="logger">Đối tượng ghi log.</param>
    /// <param name="httpContextAccessor">Đối tượng truy cập HttpContext.</param>
    public class Handler(
        ILogger logger,
        IHttpContextAccessor httpContextAccessor,
        IPermissionService permissionService) : IRequestHandler<Query, UserInfo>
    {
        /// <summary>
        /// Xử lý yêu cầu lấy thông tin người dùng từ thông tin xác thực.
        /// </summary>
        /// <param name="request">Yêu cầu lấy thông tin người dùng.</param>
        /// <param name="cancellationToken">Token hủy bỏ tác vụ.</param>
        /// <returns>Thông tin người dùng hiện tại.</returns>
        /// <exception cref="InvalidOperationException">Khi HttpContext không khả dụng.</exception>
        public Task<UserInfo> Handle(Query request, CancellationToken cancellationToken)
        {
            // Lấy HttpContext hiện tại
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            
            // Lấy thông tin người dùng từ Claims
            var user = httpContext.User;
            
            // Chuyển đổi thông tin từ Claims thành đối tượng UserInfo
            var userInfo = user.GetUserInfo();
            
            userInfo.IsLocalAccount = permissionService.IsLocalUser(user);
            
            // Ghi log thành công
            logger.Information("Lấy thông tin người sử dụng thành công {email}", userInfo.Email);
            
            // Trả về thông tin người dùng
            return Task.FromResult(userInfo);
        }
    }
    
    /// <summary>
    /// Định nghĩa endpoint API cho tính năng lấy thông tin người dùng.
    /// </summary>
    public class UserEndpoint : ICarterModule
    {
        /// <summary>
        /// Cấu hình các route cho tính năng lấy thông tin người dùng.
        /// </summary>
        /// <param name="app">Đối tượng xây dựng endpoint.</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/user/info", async (ISender sender) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query());
                    return Results.Ok(response);
                })
                .RequireAuthorization() // Yêu cầu xác thực người dùng
                .WithTags("User"); // Gắn tag để nhóm API trong Swagger
        }
    }
}