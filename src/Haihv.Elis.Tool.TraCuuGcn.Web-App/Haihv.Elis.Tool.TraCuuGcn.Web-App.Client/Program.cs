using Blazored.LocalStorage;
using Haihv.Elis.Tool.TraCuuGcn.Web_App.Client.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped<ZxingService>();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthorizationCore();

var baseUrl = builder.HostEnvironment.BaseAddress;
var appSettings = await AppSettingsService.GetAppSettings(baseUrl);
var apiEndpoint = appSettings.ApiEndpoint;
// Tải cấu hình từ Blazor Server

// Kiểm tra xem API URL hợp lệ hay không
if (!Uri.TryCreate(apiEndpoint, UriKind.Absolute, out var validUri))
{
        throw new InvalidOperationException($"Invalid API Base URL: {apiEndpoint}");
}
builder.Services.AddHttpClient(
        "Endpoint",
        opt => opt.BaseAddress = validUri);
// Đăng ký dịch vụ cấu hình ứng dụng
builder.AddAppSettingsServices(appSettings);

await builder.Build().RunAsync();