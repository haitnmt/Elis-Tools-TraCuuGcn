using System.Reflection;
using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;
using Microsoft.Extensions.Caching.Hybrid;

namespace Haihv.Elis.Tool.TraCuuGcn.WebApp.Extensions;

internal static class AppSettingsEndpoints
{
    /// <summary>
    /// Định nghĩa các endpoint cho AppSettings.
    /// </summary>
    /// <param name="app">Ứng dụng web.</param>
    public static void MapAppSettingsEndpoints(this WebApplication app)
    {
        app.MapGet(SettingUri.GetAppSetting, GetAsync)
            .WithName("GetAsync");
    }

    /// <summary>
    /// Lấy địa chỉ API từ cấu hình.
    /// </summary>
    /// <param name="appSettingsService">Dịch vụ cấu hình ứng dụng.</param>
    /// <returns></returns>
    private static async Task<IResult> GetAsync(IAppSettingsService appSettingsService)
    {
        return Results.Ok(await appSettingsService.GetAppSettingAsync());
    }
}

internal class ServerAppSettingsService(HybridCache hybridCache, IConfiguration configuration): IAppSettingsService
{
    private const string SettingsDemoKey = "IsDemo";
    private const string SettingsApiEndpointKey = "ApiEndpoint";
    
    public async Task<AppSettings> GetAppSettingAsync(CancellationToken cancellationToken = default)
    {
        const string cacheKey = "AppSettings";
        return await hybridCache.GetOrCreateAsync(cacheKey,
            async token => await GetAppSettingsAsyncFromConfiguration(token), 
            cancellationToken: cancellationToken);
    }

    private  async Task<AppSettings> GetAppSettingsAsyncFromConfiguration(CancellationToken cancellationToken = default)
    {
        var apiEndpoint = configuration[SettingsApiEndpointKey];
        var appSettings = new AppSettings
        {
            ApiEndpoint = apiEndpoint,
            IsDemoVersion = configuration[SettingsDemoKey]?.ToLower() == "true",
            AppVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString()
        };
        if (string.IsNullOrWhiteSpace(apiEndpoint)) return appSettings;
        // Lấy thông tin ApiVersion từ ApiEndpoint
        var httpClient = new HttpClient { BaseAddress = new Uri(apiEndpoint) };
        try
        {
            var response = await httpClient.GetAsync(SettingUri.GetApiVersion, cancellationToken);
            if (response.IsSuccessStatusCode)
            {            
                var apiVersion = await response.Content.ReadAsStringAsync(cancellationToken);
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

internal static class AppSettingsExtension
{
    internal static void AddServerAppSettingsServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAppSettingsService, ServerAppSettingsService>();
    }
}