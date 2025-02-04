namespace Haihv.Elis.Tool.TraCuuGcn.WebApp.Extensions;

public static class AppSettingsEndpoints
{
    /// <summary>
    /// Định nghĩa các endpoint cho AppSettings.
    /// </summary>
    /// <param name="app">Ứng dụng web.</param>
    public static void MapAppSettingsEndpoints(this WebApplication app)
    {
        app.MapGet("/api/apiEndpoint", GetApiEndpoint)
            .WithName("GetApiEndpoint");
    }
    

    /// <summary>
    /// Lấy địa chỉ API từ cấu hình.
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private static IResult GetApiEndpoint(IConfiguration configuration)
    {
        var apiEndpoint = configuration["ApiEndpoint"];
        return string.IsNullOrWhiteSpace(apiEndpoint) ? 
            Results.BadRequest("ApiEndpoint is not configured.") : 
            Results.Ok(apiEndpoint);
    }
    
}