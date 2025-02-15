using Blazored.LocalStorage;
using Haihv.Elis.Tool.TraCuuGcn.Web_App.Components;
using Haihv.Elis.Tool.TraCuuGcn.WebApp.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddScoped<ZxingService>();
builder.Services.AddScoped<LeafletMapService>();
builder.Services.AddMemoryCache();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthorizationCore();
builder.AddAppSettingsServices();
var apiEndpoint = builder.Configuration["ApiEndpoint"];
if (string.IsNullOrWhiteSpace(apiEndpoint))
{
    throw new InvalidOperationException("BackendUrl is not configured");
}
builder.Services.AddHttpClient(
    "Endpoint", client => client.BaseAddress = new Uri(apiEndpoint));



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
app.Run();