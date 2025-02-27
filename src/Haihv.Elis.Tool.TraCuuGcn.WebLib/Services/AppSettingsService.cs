using System.Net.Http.Json;
using System.Reflection;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public sealed class AppSettingsService(AppSettings appSettings)
{
    public AppSettings AppSettings { get; private set; } = appSettings;

    public static async Task<AppSettings> GetAppSettings(string baseUrl)
    {
        HttpClient httpClient = new()
        {
            BaseAddress = new Uri(baseUrl)
        };
        var appSettings =  await httpClient.GetFromJsonAsync<AppSettings>("/api/appsettings");
        if (appSettings == null)
            return new AppSettings
            {
                AppVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString(),
                IsDemoVersion = true
            };
        return appSettings;
    }
}

public sealed class AppSettings
{
    public string? ApiEndpoint { get; set; }
    public string? AuthEndpoint { get; set; }
    public string? ApiVersion { get; set; }
    public string? AppVersion { get; set; }
    public bool IsDemoVersion { get; set; }
}