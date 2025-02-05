using System.Net.Http.Json;
using System.Reflection;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public sealed class AppSettingsServices(string baseUrl)
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri(baseUrl)
    };

    public AppSettings AppSettings => GetAppSettings().Result;
    
    private async Task<AppSettings> GetAppSettings()
    {
        var appSettings =  await _httpClient.GetFromJsonAsync<AppSettings>("/api/appsettings");
        if (appSettings == null)
            return new AppSettings
            {
                AppVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString(),
                IsTrialVersion = true
            };
        return appSettings;
    }
}

public sealed class AppSettings
{
    public string? ApiEndpoint { get; set; }
    public string? ApiVersion { get; set; }
    public string? AppVersion { get; set; }
    public  bool IsTrialVersion { get; set; }
    
}