using Microsoft.Extensions.DependencyInjection;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Extensions;

public static class AppSettingsExtension
{
    public static void AddAppSettingsServices(this IServiceCollection services)
    {
        
    }
}

public sealed class AppSettingsServices(string? baseUrl = null)
{
    private readonly HttpClient _httpClient = new HttpClient(
    {
        BaseAddress = new Uri(baseUrl)
    });
    public string? ApiEndpoint { get; set; }
    public  bool IsTrialVersion { get; set; }
}