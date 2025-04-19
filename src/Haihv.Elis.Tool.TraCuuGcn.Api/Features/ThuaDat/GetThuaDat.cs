using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.ThuaDat;

/// <summary>
/// Cung cấp các tính năng lấy thông tin thửa đất dựa trên số phát hành Giấy chứng nhận.
/// </summary>
public static class GetThuaDat
{
    /// <summary>
    /// Đại diện cho một yêu cầu lấy thông tin thửa đất.
    /// </summary>
    /// <param name="Serial">Số phát hành của Giấy chứng nhận.</param>
    /// <param name="SoDinhDanh">Số định danh của chủ sử dụng (tùy chọn).</param>
    public record Query(string Serial, string? SoDinhDanh) : IRequest<IEnumerable<TraCuuGcn.Models.ThuaDat>>;

    /// <summary>
    /// Xử lý yêu cầu lấy thông tin thửa đất.
    /// </summary>
    public class Handler(
        ILogger logger,
        IPermissionService permissionService,
        IHttpContextAccessor httpContextAccessor,
        IThuaDatService thuaDatService) : IRequestHandler<Query, IEnumerable<TraCuuGcn.Models.ThuaDat>>
    {
        /// <summary>
        /// Xử lý yêu cầu lấy thông tin thửa đất và trả về danh sách thửa đất.
        /// </summary>
        /// <param name="request">Yêu cầu lấy thông tin thửa đất.</param>
        /// <param name="cancellationToken">Token hủy bỏ tác vụ.</param>
        /// <returns>Danh sách thửa đất nếu tìm thấy.</returns>
        /// <exception cref="NoSerialException">Khi không có số phát hành.</exception>
        /// <exception cref="InvalidOperationException">Khi HttpContext không khả dụng.</exception>
        /// <exception cref="UnauthorizedAccessException">Khi người dùng không có quyền đọc thông tin.</exception>
        /// <exception cref="ThuaDatNotFoundException">Khi không tìm thấy thông tin thửa đất.</exception>
        public async Task<IEnumerable<TraCuuGcn.Models.ThuaDat>> Handle(Query request, CancellationToken cancellationToken)
        {
            // Chuẩn hóa số phát hành
            var serial = request.Serial.ChuanHoa();
            if (string.IsNullOrWhiteSpace(serial))
                throw new NoSerialException();
                
            // Lấy thông tin HttpContext để kiểm tra quyền và ghi log
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var user = httpContext.User;
            
            // Kiểm tra quyền đọc dữ liệu
            if (!await permissionService.HasReadPermission(user, serial, request.SoDinhDanh, cancellationToken))
                throw new UnauthorizedAccessException();
                
            // Lấy thông tin email và URL để ghi log
            var email = user.GetEmail();
            var url = httpContext.Request.GetDisplayUrl();
            
            // Lấy thông tin Thửa Đất từ service
            var result = await thuaDatService.GetResultAsync(serial, cancellationToken);
            
            // Xử lý kết quả trả về
            return result.Match(
                // Xử lý khi lấy thông tin thành công
                thuaDats =>
                {
                    logger.Information("Lấy thông tin Thửa Đất thành công: {Url}{Email}{Serial}",
                        url,
                        email,
                        serial);
                        
                    // Kiểm tra danh sách thửa đất có dữ liệu hay không
                    if (thuaDats.Count == 0) throw new ThuaDatNotFoundException(serial);
                    
                    return thuaDats;
                },
                // Xử lý khi có lỗi
                ex =>
                {
                    logger.Error(ex, "Lỗi khi lấy thông tin Thửa Đất: {Url}{Email}{Serial}",
                        url,
                        email,
                        serial);
                    throw ex;
                });
        }
    }
    
    /// <summary>
    /// Định nghĩa các endpoint API cho tính năng lấy thông tin thửa đất.
    /// </summary>
    public class ThuaDatEndpoint : ICarterModule
    {
        /// <summary>
        /// Cấu hình các route cho tính năng lấy thông tin thửa đất.
        /// </summary>
        /// <param name="app">Đối tượng xây dựng endpoint.</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/thua-dat/", async (ISender sender, string serial, string? soDinhDanh = null) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(serial, soDinhDanh));
                    return Results.Ok(response);
                })
                .RequireAuthorization() // Yêu cầu xác thực người dùng
                .WithTags("ThuaDat"); // Gắn tag để nhóm API trong Swagger
        }
    }
}