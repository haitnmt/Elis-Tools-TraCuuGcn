using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Data;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Mvc;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class ChuSuDungEndpoints
{
    /// <summary>
    /// Định nghĩa endpoint để lấy thông tin chủ sử dụng.
    /// </summary>
    /// <param name="app">Ứng dụng web.</param>
    public static void MapChuSuDungEndpoints(this WebApplication app)
    {
        app.MapGet("/elis/csd", GetChuSuDung)
            .WithName("GetChuSuDung")
            .RequireAuthorization();
    }

    /// <summary>
    /// Lấy thông tin chủ sử dụng theo số định danh.
    /// </summary>
    /// <param name="maGcn">Mã GCN của Giấy chứng nhận.</param>
    /// <param name="httpContext">Ngữ cảnh HTTP hiện tại.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="chuSuDungService">Dịch vụ chủ sử dụng.</param>
    /// <returns>Kết quả truy vấn chủ sử dụng.</returns>
    private static async Task<IResult> GetChuSuDung(
        [FromQuery] long maGcn,
        HttpContext httpContext,
        ILogger<Program> logger,
        IChuSuDungService chuSuDungService)
    {
        // Lấy thông tin người dùng theo token từ HttpClient
        var user = httpContext.User;
        var result = await chuSuDungService.GetResultAsync(maGcn, user.GetSoDinhDanh());
        return await Task.FromResult(result.Match(
            chuSuDung => Results.Ok(new Response<ChuSuDungInfo>(chuSuDung)),
            ex => Results.BadRequest(new Response<ChuSuDungInfo>(ex.Message))));
    }
}