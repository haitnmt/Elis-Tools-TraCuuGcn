using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Data;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class ChuSuDungEndpoints
{
    /// <summary>
    /// Định nghĩa endpoint để lấy thông tin chủ sử dụng.
    /// </summary>
    /// <param name="app">Ứng dụng web.</param>
    public static void MapChuSuDungEndpoints(this WebApplication app)
    {
        app.MapGet("/elis/chuSuDung", GetChuSuDung)
            .WithName("GetChuSuDung")
            .RequireAuthorization();
    }

    /// <summary>
    /// Lấy thông tin chủ sử dụng theo số định danh.
    /// </summary>
    /// <param name="maGcnElis">Mã GCN của Giấy chứng nhận.</param>
    /// <param name="httpContext">Ngữ cảnh HTTP hiện tại.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="chuSuDungService">Dịch vụ chủ sử dụng.</param>
    /// <returns>Kết quả truy vấn chủ sử dụng.</returns>
    private static async Task<IResult> GetChuSuDung(
        [FromQuery] long maGcnElis,
        HttpContext httpContext,
        ILogger logger,
        IChuSuDungService chuSuDungService)
    {
        // Lấy thông tin người dùng theo token từ HttpClient
        var user = httpContext.User;
        var soDinhDanh = user.GetSoDinhDanh();
        var result = await chuSuDungService.GetResultAsync(maGcnElis, soDinhDanh);
        return await Task.FromResult(result.Match(
            chuSuDung =>
            {
                logger.Information("Lấy thông tin chủ sử dụng thành công: {MaGcnElis} {SoDinhDanh}", 
                    maGcnElis, soDinhDanh);
                return Results.Ok(new Response<ChuSuDungInfo>(chuSuDung));
            },
            ex =>
            {
                logger.Error(ex, "Lỗi khi lấy thông tin chủ sử dụng: {MaGcnElis} {SoDinhDanh}", 
                    maGcnElis, soDinhDanh);
                return Results.BadRequest(new Response<ChuSuDungInfo>(ex.Message));
            }));
    }
}