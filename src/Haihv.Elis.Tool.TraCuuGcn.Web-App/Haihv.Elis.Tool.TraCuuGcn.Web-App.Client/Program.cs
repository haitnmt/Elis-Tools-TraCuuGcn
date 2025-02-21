using Blazored.LocalStorage;
using Haihv.Elis.Tool.TraCuuGcn.Web_App.Client.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped<ZxingService>();
builder.Services.AddScoped<LeafletMapService>();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<JwtAuthorizationHandler>();

// Tải cấu hình từ Blazor Server
var baseUrl = builder.HostEnvironment.BaseAddress;
var appSettings = await AppSettingsService.GetAppSettings(baseUrl);
var apiEndpoint = appSettings.ApiEndpoint;
var authEndpoint = appSettings.AuthEndpoint;

// Đăng ký httpClient để gọi API
// Kiểm tra xem API URL hợp lệ hay không
if (!Uri.TryCreate(apiEndpoint, UriKind.Absolute, out var validUri))
{ 
        throw new InvalidOperationException($"Invalid API Base URL: {apiEndpoint}");
}
builder.Services.AddHttpClient(
        "Endpoint",
        opt => opt.BaseAddress = validUri)
        .AddHttpMessageHandler<JwtAuthorizationHandler>();
// Đăng ký httpClient để gọi Auth API
if (!Uri.TryCreate(authEndpoint, UriKind.Absolute, out var validAuthUri))
{
        throw new InvalidOperationException($"Invalid Auth Base URL: {authEndpoint}");
}
builder.Services.AddHttpClient(
        "AuthEndpoint",
        opt => opt.BaseAddress = validAuthUri);
// Đăng ký dịch vụ cấu hình ứng dụng
builder.AddAppSettingsServices(appSettings);

await builder.Build().RunAsync();