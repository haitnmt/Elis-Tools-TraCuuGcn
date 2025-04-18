﻿using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.GiayChungNhan;

/// <summary>
/// Tính năng lấy thông tin chi tiết Giấy chứng nhận
/// </summary>
/// <remarks>
/// Tính năng này cho phép truy vấn thông tin chi tiết của Giấy chứng nhận
/// dựa trên số Serial được cung cấp
/// </remarks>
public static class GetGiayChungNhan
{
    /// <summary>
    /// Truy vấn thông tin Giấy chứng nhận
    /// </summary>
    /// <param name="Serial">Số Serial của Giấy chứng nhận cần truy vấn</param>
    public record Query(string Serial) : IRequest<TraCuuGcn.Models.GiayChungNhan>;
    
    /// <summary>
    /// Xử lý truy vấn thông tin Giấy chứng nhận
    /// </summary>
    /// <remarks>
    /// Lớp này thực hiện việc lấy thông tin chi tiết Giấy chứng nhận
    /// từ cơ sở dữ liệu và ghi log quá trình truy vấn
    /// </remarks>
    /// <param name="logger">Dịch vụ ghi log</param>
    /// <param name="httpContextAccessor">Đối tượng truy cập HttpContext</param>
    /// <param name="giayChungNhanService">Dịch vụ quản lý Giấy chứng nhận</param>
    public class Handler(ILogger logger,
        IHttpContextAccessor httpContextAccessor,
        IGiayChungNhanService giayChungNhanService) : IRequestHandler<Query, TraCuuGcn.Models.GiayChungNhan>
    {
        /// <summary>
        /// Xử lý yêu cầu lấy thông tin Giấy chứng nhận
        /// </summary>
        /// <param name="request">Truy vấn chứa số Serial của Giấy chứng nhận</param>
        /// <param name="cancellationToken">Token hủy thao tác</param>
        /// <returns>Đối tượng Giấy chứng nhận chứa thông tin chi tiết</returns>
        /// <exception cref="InvalidOperationException">Ném ra khi HttpContext không khả dụng</exception>
        /// <exception cref="NoSerialException">Ném ra khi số Serial không hợp lệ</exception>
        public async Task<TraCuuGcn.Models.GiayChungNhan> Handle(Query request, CancellationToken cancellationToken)
        {
            // Lấy HttpContext, ném ngoại lệ nếu không có
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            
            // Lấy địa chỉ IP và URL yêu cầu cho mục đích ghi log
            var ipAddress = httpContext.GetIpAddress();
            var url = httpContext.Request.GetDisplayUrl();
            var serial = request.Serial;
            
            // Kiểm tra tính hợp lệ của số Serial
            if (string.IsNullOrWhiteSpace(serial))
                throw new NoSerialException();
            
            // Thực hiện truy vấn thông tin Giấy chứng nhận
            var result = await giayChungNhanService.GetResultAsync(serial, cancellationToken: cancellationToken);
            
            // Xử lý kết quả truy vấn
            return result.Match(giayChungNhan =>
            {
                // Ghi log thành công
                logger.Information("Lấy thông tin Giấy Chứng Nhận thành công: {Url}{ClientIp}{Serial}",
                    url,
                    ipAddress,
                    serial);
                return giayChungNhan;
            },
            ex =>
            {
                // Ghi log lỗi và ném lại ngoại lệ
                logger.Error(ex, "Lỗi khi lấy thông tin Giấy Chứng Nhận: {Url}{ClientIp}{Serial}", 
                    url, ipAddress, serial);
                throw ex;
            });
        }
    }
    
    /// <summary>
    /// Cấu hình endpoint cho tính năng lấy thông tin Giấy chứng nhận
    /// </summary>
    /// <remarks>
    /// Lớp này định nghĩa các endpoint API cho việc truy vấn thông tin Giấy chứng nhận
    /// </remarks>
    public class GiayChungNhanEndpoint : ICarterModule
    {
        /// <summary>
        /// Đăng ký các route cho tính năng lấy thông tin Giấy chứng nhận
        /// </summary>
        /// <param name="app">Đối tượng cấu hình endpoint</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/giay-chung-nhan/", async (ISender sender, string serial) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(serial));
                    return Results.Ok(response);
                })
                .RequireAuthorization() // Yêu cầu xác thực
                .WithTags("GiayChungNhan"); // Gắn tag cho Swagger
        }
    }
}