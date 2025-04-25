using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.ThuaDat;

/// <summary>
/// Cung cấp các tính năng lấy thông tin công khai về thửa đất dựa trên số phát hành Giấy chứng nhận.
/// Thông tin công khai chỉ bao gồm các dữ liệu cơ bản về thửa đất, không chứa thông tin cá nhân và nhạy cảm.
/// </summary>
public static class GetThuaDatPublic
{
    /// <summary>
    /// Đại diện cho một yêu cầu lấy thông tin công khai về thửa đất.
    /// </summary>
    /// <param name="Serial">Số phát hành của Giấy chứng nhận.</param>
    public record Query(string Serial) : IRequest<IEnumerable<ThuaDatPublic>>;

    /// <summary>
    /// Xử lý yêu cầu lấy thông tin công khai về thửa đất.
    /// </summary>
    public class Handler(
        ILogger logger,
        IHttpContextAccessor httpContextAccessor,
        IThuaDatService thuaDatService) : IRequestHandler<Query, IEnumerable<ThuaDatPublic>>
    {
        /// <summary>
        /// Xử lý yêu cầu lấy thông tin công khai về thửa đất và trả về danh sách thông tin công khai.
        /// </summary>
        /// <param name="request">Yêu cầu lấy thông tin công khai về thửa đất.</param>
        /// <param name="cancellationToken">Token hủy bỏ tác vụ.</param>
        /// <returns>Danh sách thông tin công khai về thửa đất nếu tìm thấy.</returns>
        /// <exception cref="NoSerialException">Khi không có số phát hành.</exception>
        /// <exception cref="InvalidOperationException">Khi HttpContext không khả dụng.</exception>
        public async Task<IEnumerable<ThuaDatPublic>> Handle(Query request, CancellationToken cancellationToken)
        {
            // Chuẩn hóa số phát hành
            var serial = request.Serial.ChuanHoa();
            if (string.IsNullOrWhiteSpace(serial))
                throw new NoSerialException();
                
            // Lấy thông tin HttpContext để ghi log
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
                              
            // Lấy thông tin email và URL để ghi log
            var email = httpContext.User.GetEmail();
            var url = httpContext.Request.GetDisplayUrl();
            
            // Lấy thông tin Thửa Đất từ service
            var result = await thuaDatService.GetResultAsync(serial, cancellationToken);
            
            // Xử lý kết quả trả về
            return result.Match(
                // Xử lý khi lấy thông tin thành công
                thuaDats =>
                {
                    logger.Information("Lấy thông tin Thửa Đất công khai thành công: {Url}{Email}{Serial}",
                        url,
                        email,
                        serial);
                        
                    // Chuyển đổi danh sách thửa đất sang định dạng công khai
                    var thuaDatPublic = thuaDats.Select(x => x.ConvertToThuaDatPublic());
                    return thuaDatPublic;
                },
                // Xử lý khi có lỗi
                ex =>
                {
                    logger.Error(ex, "Lỗi khi lấy thông tin Thửa Đất công khai: {Url}{Email}{Serial}",
                        url,
                        email,
                        serial);
                    throw ex;
                });
        }
    }
    
    /// <summary>
    /// Định nghĩa các endpoint API cho tính năng lấy thông tin công khai về thửa đất.
    /// </summary>
    public class ThuaDatEndpoint : ICarterModule
    {
        /// <summary>
        /// Cấu hình các route cho tính năng lấy thông tin công khai về thửa đất.
        /// </summary>
        /// <param name="app">Đối tượng xây dựng endpoint.</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(ThuaDatUri.GetThuaDatPublic, async (ISender sender, string serial) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(serial));
                    return Results.Ok(response);
                })
                .WithTags("ThuaDat"); // Gắn tag để nhóm API trong Swagger
        }
    }
}