using Haihv.Elis.Tool.TraCuuGcn.WebLib;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices();
builder.Services.AddHttpClient(
        "Auth",
        opt => opt.BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:7263/"));
builder.Services.AddScoped<ZxingService>();
await builder.Build().RunAsync();