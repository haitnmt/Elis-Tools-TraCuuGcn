using Blazored.LocalStorage;
using Haihv.Elis.Tool.TraCuuGcn.Web_App.Client.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped<ZxingService>();
builder.Services.AddScoped<BarcodeDetectionService>();
builder.Services.AddScoped<LeafletMapService>();
builder.Services.AddMudServices(config =>
{
        config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
        config.SnackbarConfiguration.PreventDuplicates = false;
        config.SnackbarConfiguration.NewestOnTop = false;
        config.SnackbarConfiguration.ShowCloseIcon = true;
        config.SnackbarConfiguration.MaxDisplayedSnackbars = 10;
        config.SnackbarConfiguration.VisibleStateDuration = 10000;
        config.SnackbarConfiguration.HideTransitionDuration = 500;
        config.SnackbarConfiguration.ShowTransitionDuration = 500;
        config.SnackbarConfiguration.SnackbarVariant = Variant.Outlined;
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthEndpointCookieHandler>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
builder.Services.AddScoped<IStorageSyncService, StorageSyncService>();
builder.Services.AddAuthorizationCore();



// Tải cấu hình từ Blazor Server
var baseUrl = builder.HostEnvironment.BaseAddress;
var appSettings = await AppSettingsService.GetAppSettings(baseUrl);
var apiEndpoint = appSettings.ApiEndpoint;
var authEndpoint = appSettings.AuthEndpoint;

// Đăng ký dịch vụ cấu hình ứng dụng
builder.AddAppSettingsServices(appSettings);

// Đăng ký httpClient để gọi API
// Kiểm tra xem API URL hợp lệ hay không
if (!Uri.TryCreate(apiEndpoint, UriKind.Absolute, out var validUri))
{
        throw new InvalidOperationException($"Invalid API Base URL: {apiEndpoint}");
}

builder.Services.AddHttpClient(
        "Endpoint",
        opt => opt.BaseAddress = validUri);
// Đăng ký httpClient để gọi Auth API
if (!Uri.TryCreate(authEndpoint, UriKind.Absolute, out var validAuthUri))
{
        throw new InvalidOperationException($"Invalid Auth Base URL: {authEndpoint}");
}

// Cấu hình HttpClient cho AuthEndpoint với hỗ trợ cookie
builder.Services.AddHttpClient(
        "AuthEndpoint",
        opt =>
        {
                opt.BaseAddress = validAuthUri;
        }).AddHttpMessageHandler<AuthEndpointCookieHandler>();
        

await builder.Build().RunAsync();