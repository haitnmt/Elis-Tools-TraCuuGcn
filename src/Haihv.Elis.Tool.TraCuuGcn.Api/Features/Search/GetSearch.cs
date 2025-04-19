﻿using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.Search;

/// <summary>
/// Cung cấp các tính năng tìm kiếm thông tin Giấy chứng nhận.
/// </summary>
public static class GetSearch
{
    /// <summary>
    /// Đại diện cho một yêu cầu tìm kiếm Giấy chứng nhận.
    /// </summary>
    /// <param name="Search">Chuỗi tìm kiếm (có thể là số serial, số phát hành, hoặc thông tin khác của GCN).</param>
    public record Query(string Search) : IRequest<GiayChungNhanInfo>;
    
    /// <summary>
    /// Xử lý yêu cầu tìm kiếm thông tin Giấy chứng nhận.
    /// </summary>
    public class Handler(ISearchService searchService,
        IThuaDatService thuaDatService,
        IChuSuDungService chuSuDungService,
        ITaiSanService taiSanService,
        IGeoService geoService,
        ILogger logger,
        IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<Query, GiayChungNhanInfo>
    {
        /// <summary>
        /// Xử lý yêu cầu tìm kiếm và trả về thông tin Giấy chứng nhận.
        /// </summary>
        /// <param name="request">Yêu cầu tìm kiếm.</param>
        /// <param name="cancellationToken">Token hủy bỏ tác vụ.</param>
        /// <returns>Thông tin Giấy chứng nhận nếu tìm thấy.</returns>
        /// <exception cref="InvalidOperationException">Khi HttpContext không khả dụng.</exception>
        /// <exception cref="SearchNotFoundException">Khi không tìm thấy thông tin GCN.</exception>
        /// <exception cref="SearchFailedException">Khi xảy ra lỗi trong quá trình tìm kiếm.</exception>
        public async Task<GiayChungNhanInfo> Handle(Query request, CancellationToken cancellationToken)
        {
            // Lấy thông tin từ HttpContext để ghi log
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var ipAddress = httpContext.GetIpAddress();
            var email = httpContext.User.GetEmail();
            var url = httpContext.Request.GetDisplayUrl();
            var query = request.Search;
            
            // Thực hiện tìm kiếm thông qua service
            var result = await searchService.GetResultAsync(query, cancellationToken);
            
            // Xử lý kết quả tìm kiếm
            return result.Match<GiayChungNhanInfo>(
                // Xử lý khi tìm kiếm thành công
                info =>
                {
                    logger.Information("Tìm kiếm thông tin thành công: {Url}{ClientIp}{Email}",
                        url,
                        ipAddress, 
                        email);
                    
                    // Kiểm tra và chuẩn hóa số serial
                    var serial = info.Serial?.ChuanHoa();
                    if (string.IsNullOrWhiteSpace(serial))
                    {
                        logger.Warning("Không tìm thấy thông tin GCN: {Url}{ClientIp}{Email}",
                            url,
                            ipAddress, 
                            email);
                        throw new SearchNotFoundException(query);
                    }
                    
                    // Bắt đầu các tác vụ không đồng bộ để cập nhật cache
                    // Lưu thông tin chủ sử dụng vào cache
                    _ = chuSuDungService.GetInDatabaseAsync(serial, cancellationToken);
                    // Lưu thông tin tài sản vào cache
                    _ = taiSanService.SetCacheAsync(serial, thuaDatService, chuSuDungService);
                    // Lưu thông tin toạ độ thửa đất vào cache
                    _ = geoService.GetAsync(serial, cancellationToken);
                    
                    return info;
                },
                // Xử lý khi tìm kiếm thất bại
                ex =>
                {
                    logger.Error(ex, "Lỗi khi tìm kiếm thông tin: {Url}{ClientIp}{Email}",
                        url,
                        ipAddress, 
                        email);
                    throw new SearchFailedException(ex.Message);
                });
        }
    }
    
    /// <summary>
    /// Định nghĩa các endpoint API cho tính năng tìm kiếm.
    /// </summary>
    public class SearchEndpoint : ICarterModule
    {
        /// <summary>
        /// Cấu hình các route cho tính năng tìm kiếm.
        /// </summary>
        /// <param name="app">Đối tượng xây dựng endpoint.</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/search", async (ISender sender, string query) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(query));
                    return Results.Ok(response);
                })
                .RequireAuthorization() // Yêu cầu xác thực người dùng
                .WithTags("Search"); // Gắn tag để nhóm API trong Swagger
        }
    }
}