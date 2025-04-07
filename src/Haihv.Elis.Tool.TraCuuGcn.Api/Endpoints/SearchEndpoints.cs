using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class SearchEndpoints
{
    public static void MapSearchEndpoints(this WebApplication app)
    {
        app.MapGet("/elis/search", GetSearchResultAsync)
            .WithName("GetGcnInfoAsync");
    }

    private static async Task<IResult> GetSearchResultAsync(
        ISearchService searchService,
        IThuaDatService thuaDatService,
        IChuSuDungService chuSuDungService,
        ITaiSanService taiSanService,
        IGeoService geoService,
        ILogger logger,
        HttpContext httpContext)
    {
        var query = httpContext.Request.Query["query"].ToString();
        var ipAddress = httpContext.GetIpAddress();
        var url = httpContext.Request.GetDisplayUrl();
        var result = await searchService.GetResultAsync(query);
        return await Task.FromResult(result.Match(
            info =>
            {
                logger.Information("Tìm kiếm thông tin thành công: {Url}{ClientIp}",
                    url,
                    ipAddress);
                // Lưu thông tin chủ sử dụng để lưu vào cache
                var serial = info.Serial?.ChuanHoa();
                if (string.IsNullOrWhiteSpace(serial))
                {
                    logger.Warning("Không tìm thấy thông tin GCN: {Url}{ClientIp}",
                        url,
                        ipAddress);
                    return Results.NotFound(new Response<GiayChungNhanInfo>("Không tìm thấy thông tin GCN!"));
                }
                _ = chuSuDungService.SetCacheAuthChuSuDungAsync(serial);
                _ = chuSuDungService.GetAsync(serial);
                _ = taiSanService.SetCacheAsync(serial, thuaDatService, chuSuDungService);
                // Lưu thông tin toạ độ thửa đất để lưu vào cache
                _ = geoService.GetAsync(serial);
                return Results.Ok(new Response<GiayChungNhanInfo>(info));
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi tìm kiếm thông tin: {Url}{ClientIp}",
                    url,
                    ipAddress);
                return Results.BadRequest(new Response<GiayChungNhanInfo>(ex.Message));
            }));
    }

}