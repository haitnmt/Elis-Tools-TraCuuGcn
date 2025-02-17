using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class SearchEndpoints
{
    public static void MapSearchEndpoints(this WebApplication app)
    {
        app.MapGet("/elis/search", GetSearchResultAsync)
            .WithName("GetGcnInfoAsync");
    }

    private static async Task<IResult> GetSearchResultAsync([FromQuery] string? query,
        ISearchService searchService,
        IChuSuDungService chuSuDungService,
        IGeoService geoService,
        ILogger logger)
    {
        var result = await searchService.GetResultAsync(query);
        return await Task.FromResult(result.Match(
            info=>
            {
                logger.Information("Lấy thông tin Giấy chứng nhận thành công: {Serial}", info.Serial);
                // Lưu thông tin chủ sử dụng để lưu vào cache
                _ = chuSuDungService.SetCacheAuthChuSuDungAsync(info.MaGcnElis);
                _ = chuSuDungService.SetCacheAsync(info.MaGcnElis);
                // Lưu thông tin toạ độ thửa đất để lưu vào cache
                _ = geoService.GetAsync(info.MaGcnElis);
                return Results.Ok(new Response<GiayChungNhanInfo>(info));
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi lấy thông tin Giấy chứng nhận: {Query}", query);
                return Results.BadRequest(new Response<GiayChungNhanInfo>(ex.Message));
            }));
    }
    
}