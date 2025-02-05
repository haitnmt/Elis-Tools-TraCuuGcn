using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Haihv.Elis.Tool.TraCuuGcn.Web_App.Client.Extensions;

internal static class AppSettingsExtension
{
    public static void AddAppSettingsServices(this WebAssemblyHostBuilder builder, AppSettings appSettings)
    {
        builder.Services.AddScoped<AppSettingsService>(_ => new AppSettingsService(appSettings));
    }
}