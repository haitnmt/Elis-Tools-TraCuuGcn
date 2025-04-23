using System.Net.Http.Headers;
using System.Security.Claims;
using Haihv.Elis.Tool.TraCuuGcn.Api.Uri;
using Haihv.Elis.Tool.TraCuuGcn.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Web_App.Components;
using Haihv.Elis.Tool.TraCuuGcn.Web.App.Authentication;
using Haihv.Elis.Tool.TraCuuGcn.WebApp.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor;
using MudBlazor.Services;
using ServiceDefaults;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add Caching
builder.AddCache();

var oidcConfig = builder.Configuration.GetSection("OpenIdConnect");
const string oidcScheme = LoginLogoutEndpointRouteBuilderExtensions.OidcSchemeName;
// Add services to the container.
builder.Services.AddAuthentication(oidcScheme)
    .AddOpenIdConnect(oidcScheme, oidcOptions =>
    {
        oidcOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        oidcOptions.Authority = oidcConfig["Authority"];
        oidcOptions.ClientId = oidcConfig["ClientId"];
        oidcOptions.ClientSecret = oidcConfig["ClientSecret"];
        oidcOptions.ResponseType = oidcConfig["ResponseType"] ?? "code";
        oidcOptions.Scope.Clear();
        foreach (var scope in (oidcConfig["Scope"] ?? "openid profile email").Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            oidcOptions.Scope.Add(scope);
        }
        oidcOptions.MapInboundClaims = false;
        oidcOptions.TokenValidationParameters.NameClaimType = "name";
        oidcOptions.TokenValidationParameters.RoleClaimType = "roles";
        oidcOptions.SaveTokens = true;
        oidcOptions.Events.OnTokenValidated = context =>
        {
            // Trích xuất roles từ token
            const string roleClaimType = "resource_access.{client-id}.roles";
            var roleClaims = context.SecurityToken.Claims
                .Where(c => c.Type == roleClaimType || c.Type.EndsWith("/roles"))
                .ToList();

            if (roleClaims.Count == 0 || context.Principal?.Identity is not ClaimsIdentity identity)
                return Task.CompletedTask;
            foreach (var roleClaim in roleClaims)
            {
                identity.AddClaim(new Claim("role", roleClaim.Value));
            }

            return Task.CompletedTask;
        };

    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

builder.Services.ConfigureCookieOidcRefresh(CookieAuthenticationDefaults.AuthenticationScheme, oidcScheme);
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddHttpForwarderWithServiceDiscovery();
builder.Services.AddHttpContextAccessor();


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

builder.AddServerAppSettingsServices();
var apiEndpoint = builder.Configuration["ApiEndpoint"];
if (string.IsNullOrWhiteSpace(apiEndpoint))
{
    throw new InvalidOperationException("BackendUrl is not configured");
}
// Kiểm tra xem API URL hợp lệ hay không
if (!Uri.TryCreate(apiEndpoint, UriKind.Absolute, out var clientBaseAddress))
{
    throw new InvalidOperationException($"Invalid API Base URL: {apiEndpoint}");
}

builder.Services.AddHttpClient<IUserServices, ServerUserServices>(
    client => client.BaseAddress = clientBaseAddress);
builder.Services.AddHttpClient<IGiayChungNhanServices, ServerGiayChungNhanServices>(
    client => client.BaseAddress = clientBaseAddress);
builder.Services.AddHttpClient<ICacheService, ServerCacheServices>(
    client => client.BaseAddress = clientBaseAddress);
builder.Services.AddHttpClient<IThuaDatServices, ServerThuaDatService>(
    client=> client.BaseAddress = clientBaseAddress);
builder.Services.AddHttpClient<IChuSuDungServices, ServerChuSuDungService>(
    client=> client.BaseAddress = clientBaseAddress);
builder.Services.AddHttpClient<ITaiSanServices, ServerTaiSanService>(
    client=> client.BaseAddress = clientBaseAddress);


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

app.MapDefaultEndpoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Haihv.Elis.Tool.TraCuuGcn.Web_App.Client._Imports).Assembly);

app.MapForwarder(GiayChungNhanUri.Search, apiEndpoint);
app.MapForwarder(ThuaDatUri.GetThuaDatPublic, apiEndpoint);

app.MapForwarder("/api/{**catch-all}", apiEndpoint, transformBuilder =>
{
    transformBuilder.AddRequestTransform(async transformContext =>
    {
        var accessToken = await transformContext.HttpContext.GetTokenAsync("access_token");
        transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    });
}).RequireAuthorization();

app.MapAppSettingsEndpoints();

app.MapGroup("/authentication").MapLoginAndLogout();

app.Run();