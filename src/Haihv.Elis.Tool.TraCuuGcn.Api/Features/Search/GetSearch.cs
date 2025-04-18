using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Exceptions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Features.Search;

public static class GetSearch
{
    public record Query(string Search) : IRequest<GiayChungNhanInfo>;
    public class Handler(ISearchService searchService,
        IThuaDatService thuaDatService,
        IChuSuDungService chuSuDungService,
        ITaiSanService taiSanService,
        IGeoService geoService,
        ILogger logger,
        IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<Query, GiayChungNhanInfo>
    {
        public async Task<GiayChungNhanInfo> Handle(Query request, CancellationToken cancellationToken)
        {
            var httpContext = httpContextAccessor.HttpContext
                              ?? throw new InvalidOperationException("HttpContext không khả dụng");
            var ipAddress = httpContext.GetIpAddress();
            var email = httpContext.User.GetEmail();
            var url = httpContext.Request.GetDisplayUrl();
            var query = request.Search;
            var result = await searchService.GetResultAsync(query, cancellationToken);
            return result.Match<GiayChungNhanInfo>(
                info =>
                {
                    logger.Information("Tìm kiếm thông tin thành công: {Url}{ClientIp}{Email}",
                        url,
                        ipAddress, 
                        email); // Lưu thông tin chủ sử dụng để lưu vào cache
                    var serial = info.Serial?.ChuanHoa();
                    if (string.IsNullOrWhiteSpace(serial))
                    {
                        logger.Warning("Không tìm thấy thông tin GCN: {Url}{ClientIp}{Email}",
                            url,
                            ipAddress, 
                            email);
                        throw new SearchNotFoundException(query);
                    }
                    _ = chuSuDungService.GetInDatabaseAsync(serial, cancellationToken);
                    _ = taiSanService.SetCacheAsync(serial, thuaDatService, chuSuDungService);
                    // Lưu thông tin toạ độ thửa đất để lưu vào cache
                    _ = geoService.GetAsync(serial, cancellationToken);
                    return info;
                },
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
    public class SearchEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/search", async (ISender sender, string query) =>
                {
                    // Không cần try-catch ở đây vì đã có middleware xử lý exception toàn cục
                    var response = await sender.Send(new Query(query));
                    return Results.Ok(response);
                })
                .RequireAuthorization()
                .WithTags("Search");
        }
    }
}