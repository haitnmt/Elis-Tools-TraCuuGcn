using System.Net.Http.Json;
using System.Reflection;
using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Haihv.Elis.Tool.TraCuuGcn.WebApp.Services;

public sealed class ClientAppSettingsService(string baseUrl): IAppSettingsService
{
    public async Task<AppSettings> GetAppSettingAsync(CancellationToken cancellationToken = default)
    {
        HttpClient httpClient = new()
        {
            BaseAddress = new Uri(baseUrl)
        };
        var appSettings =  await httpClient.GetFromJsonAsync<AppSettings>(SettingUri.GetAppSetting, cancellationToken: cancellationToken);
        if (appSettings == null)
            return new AppSettings
            {
                AppVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString(),
                IsDemoVersion = true
            };
        return appSettings;
    }
    
}

internal static class AppSettingsExtension
{
    public static void AddClientAppSettingsServices(this WebAssemblyHostBuilder builder, string baseUrl)
    {
        builder.Services.AddScoped<IAppSettingsService, ClientAppSettingsService>(_ => new ClientAppSettingsService(baseUrl));
    }
}