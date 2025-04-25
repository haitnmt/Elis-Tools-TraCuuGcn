using Haihv.Elis.Tool.TraCuuGcn.Web_App.Client.Services;
using Haihv.Elis.Tool.TraCuuGcn.WebApp.Client.Services;
using Haihv.Elis.Tool.TraCuuGcn.WebApp.Services;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;
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

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

// Tải cấu hình từ Blazor Server
var baseUrl = builder.HostEnvironment.BaseAddress;
// Đăng ký dịch vụ cấu hình ứng dụng
builder.AddClientAppSettingsServices(baseUrl);

builder.Services.AddHttpClient<IUserServices, ClientUserServices>(
    httpClient => httpClient.BaseAddress = new Uri(baseUrl));
builder.Services.AddHttpClient<IGiayChungNhanServices, ClientGiayChungNhanServices>(
    httpClient => httpClient.BaseAddress = new Uri(baseUrl));
builder.Services.AddHttpClient<ICacheService, ClientCacheService>(
    httpClient => httpClient.BaseAddress = new Uri(baseUrl));
builder.Services.AddHttpClient<IThuaDatServices, ClientThuaDatService>(
    httpClient => httpClient.BaseAddress = new Uri(baseUrl));
builder.Services.AddHttpClient<IChuSuDungServices, ClientChuSuDungService>(
    httpClient => httpClient.BaseAddress = new Uri(baseUrl));
builder.Services.AddHttpClient<ITaiSanServices, ClientTaiSanService>(
    httpClient => httpClient.BaseAddress = new Uri(baseUrl));
await builder.Build().RunAsync();