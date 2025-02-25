using System.Reflection;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Haihv.Elis.Tool.TraCuuGcn.WebApp.Extensions;

internal static class AppSettingsEndpoints
{
    /// <summary>
    /// Định nghĩa các endpoint cho AppSettings.
    /// </summary>
    /// <param name="app">Ứng dụng web.</param>
    public static void MapAppSettingsEndpoints(this WebApplication app)
    {
        app.MapGet("/api/appsettings", GetAsync)
            .WithName("GetAsync");
    }

    /// <summary>
    /// Lấy địa chỉ API từ cấu hình.
    /// </summary>
    /// <param name="appSettingsService">Dịch vụ cấu hình ứng dụng.</param>
    /// <returns></returns>
    private static IResult GetAsync(AppSettingsService appSettingsService)
    {
        return Results.Ok(appSettingsService.AppSettings);
    }
}
internal static class AppSettingsExtension
{
    private const string SettingsDemoKey = "IsDemo";
    private const string SettingsApiEndpointKey = "ApiEndpoint";
    private const string SettingsAuthEndpointKey = "AuthEndpoint";
    internal static void AddAppSettingsServices(this WebApplicationBuilder builder)
    {
        var appSettings = builder.Configuration.GetAppSettingsAsync().Result;
        builder.Services.AddScoped<AppSettingsService>(_ => new AppSettingsService(appSettings));
    }

    internal static async Task<AppSettings> GetAppSettingsAsync(this IMemoryCache memoryCache,
        IConfiguration configuration)
    {
        const string cacheKey = "AppSettings";
        return await memoryCache.GetOrCreateAsync(cacheKey, _ => configuration.GetAppSettingsAsync()) ?? new AppSettings();
    }

    private static async Task<AppSettings> GetAppSettingsAsync(this IConfiguration configuration)
    {
        var apiEndpoint = configuration[SettingsApiEndpointKey];
        var appSettings = new AppSettings
        {
            ApiEndpoint = apiEndpoint,
            AuthEndpoint = configuration[SettingsAuthEndpointKey],
            IsDemoVersion = configuration[SettingsDemoKey]?.ToLower() == "true",
            AppVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString()
        };
        if (string.IsNullOrWhiteSpace(apiEndpoint)) return appSettings;
        // Lấy thông tin ApiVersion từ ApiEndpoint
        var httpClient = new HttpClient { BaseAddress = new Uri(apiEndpoint) };
        try
        {
            var response = await httpClient.GetAsync("/api/version");
            if (response.IsSuccessStatusCode)
            {            
                var apiVersion = await response.Content.ReadAsStringAsync();
                apiVersion = apiVersion.Trim().Trim('"');
                appSettings.ApiVersion = apiVersion;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return appSettings;
    }
}