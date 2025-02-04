using Blazored.LocalStorage;
using Haihv.Elis.Tool.TraCuuGcn.WebLib;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices(
        config =>
        {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 10000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthorizationCore();

// Tải cấu hình từ Blazor Server
var httpClient = new HttpClient{ BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var response = await httpClient.GetStringAsync("/api/apiEndpoint");
var apiEndpoint = response.Trim('"');
// Kiểm tra xem API URL hợp lệ hay không
if (!Uri.TryCreate(apiEndpoint, UriKind.Absolute, out var validUri))
{
        throw new InvalidOperationException($"Invalid API Base URL: {apiEndpoint}");
}
builder.Services.AddHttpClient(
        "Endpoint",
        opt => opt.BaseAddress = validUri);


builder.Services.AddScoped<ZxingService>();

await builder.Build().RunAsync();