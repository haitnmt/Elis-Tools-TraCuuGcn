using Blazored.LocalStorage;
using Haihv.Elis.Tool.TraCuuGcn.Web_App.Components;
using Haihv.Elis.Tool.TraCuuGcn.WebApp.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Services;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddScoped<ZxingService>();
builder.Services.AddScoped<BarcodeDetectionService>();
builder.Services.AddScoped<LeafletMapService>();
builder.Services.AddMemoryCache();
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
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<JwtAuthorizationHandler>();
builder.Services.AddAuthorizationCore();

builder.AddAppSettingsServices();
var apiEndpoint = builder.Configuration["ApiEndpoint"];
if (string.IsNullOrWhiteSpace(apiEndpoint))
{
    throw new InvalidOperationException("BackendUrl is not configured");
}
// Kiểm tra xem API URL hợp lệ hay không
if (!Uri.TryCreate(apiEndpoint, UriKind.Absolute, out var validUri))
{
    throw new InvalidOperationException($"Invalid API Base URL: {apiEndpoint}");
}

builder.Services.AddHttpClient(
        "Endpoint",
        opt => opt.BaseAddress = validUri)
    .AddHttpMessageHandler<JwtAuthorizationHandler>();

var authEndpoint = builder.Configuration["AuthEndpoint"];
if (string.IsNullOrWhiteSpace(authEndpoint))
{
    throw new InvalidOperationException("AuthEndpoint is not configured");
}
builder.Services.AddHttpClient(
    "AuthEndpoint", client => client.BaseAddress = new Uri(authEndpoint));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Haihv.Elis.Tool.TraCuuGcn.Web_App.Client._Imports).Assembly);

app.MapAppSettingsEndpoints();
// Thêm Endpoint kiểm tra ứng dụng hoạt động
app.MapGet("/health", () => Results.Ok("OK")).WithName("GetHealth");
app.Run();