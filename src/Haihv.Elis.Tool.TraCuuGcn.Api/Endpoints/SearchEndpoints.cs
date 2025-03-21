using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Api.Settings;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class SearchEndpoints
{
    private const string UrlGetSearchResult = "/elis/search";
    public static void MapSearchEndpoints(this WebApplication app)
    {
        app.MapGet(UrlGetSearchResult, GetSearchResultAsync)
            .WithName("GetGcnInfoAsync");
    }
    
    private static async Task<IResult> GetSearchResultAsync([FromQuery] string? query,
        ISearchService searchService,
        IChuSuDungService chuSuDungService,
        IGeoService geoService,
        ILogger logger,
        HttpContext httpContext)
    {
        var result = await searchService.GetResultAsync(query);
        var ipAddress = httpContext.GetIpAddress();
        return await Task.FromResult(result.Match(
            info=>
            {
                logger.Information("Tìm kiếm thông tin thành công: {Query}{Url}{ClientIp}", 
                    query, 
                    UrlGetSearchResult, 
                    ipAddress);
                // Lưu thông tin chủ sử dụng để lưu vào cache
                var serial = info.Serial?.ChuanHoa();
                if (string.IsNullOrWhiteSpace(serial))
                {
                    logger.Warning("Không tìm thấy thông tin GCN: {Query}{Url}{ClientIp}", 
                        query, 
                        UrlGetSearchResult, 
                        ipAddress);
                    return Results.NotFound(new Response<GiayChungNhanInfo>("Không tìm thấy thông tin GCN!"));
                }
                _ = chuSuDungService.SetCacheAuthChuSuDungAsync(serial);
                _ = chuSuDungService.GetAsync(serial);
                // Lưu thông tin toạ độ thửa đất để lưu vào cache
                _ = geoService.GetAsync(serial);
                return Results.Ok(new Response<GiayChungNhanInfo>(info));
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi tìm kiếm thông tin: {Query}{Url}{ClientIp}", 
                    query, 
                    UrlGetSearchResult, 
                    ipAddress);
                return Results.BadRequest(new Response<GiayChungNhanInfo>(ex.Message));
            }));
    }
    
}