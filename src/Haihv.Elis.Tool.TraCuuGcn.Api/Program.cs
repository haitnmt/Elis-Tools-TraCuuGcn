using System.Text;
using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Haihv.Elis.Tool.TraCuuGcn.Extensions;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

// Set Console Support Vietnamese
Console.OutputEncoding = Encoding.UTF8;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Serilog
builder.AddLogToElasticsearch();

// Add Caching
builder.AddCache();

var openIdConnectConfig = builder.Configuration.GetSection("OpenIdConnect");
var authority = openIdConnectConfig["Authority"];
var audience = openIdConnectConfig["Audience"];

builder.Services.AddAuthentication()
    .AddJwtBearer("Bearer", jwtOptions =>
    {
        jwtOptions.Authority = authority;
        jwtOptions.Audience = audience;
        jwtOptions.RequireHttpsMetadata = true;
    });
builder.Services.AddAuthorization();
builder.Services.AddSingleton<IPermissionService, PermissionService>();
// Add ConnectionElisData
builder.Services.AddSingleton<IConnectionElisData, ConnectionElisData>();

// Add WriteLogService
builder.Services.AddSingleton<ILogElisDataServices, LogElisDataServices>();

// Add GcnQrService
builder.Services.AddSingleton<IGcnQrService, GcnQrService>();
// Add GiayChungNhanService
builder.Services.AddSingleton<IGiayChungNhanService, GiayChungNhanService>();
// Add ChuSuDungService
builder.Services.AddSingleton<IChuSuDungService, ChuSuDungService>();
// Add ThuaDatService
builder.Services.AddSingleton<IThuaDatService, ThuaDatService>();
// Add TaiSanService
builder.Services.AddSingleton<ITaiSanService, TaiSanService>();
// Add HttpClient for SDE API
builder.Services.AddHttpClient("SdeApi");
// Add HttpClient for LDAP API
builder.Services.AddHttpClient("LdapApi");

// Add GeoService
builder.Services.AddSingleton<IGeoService, GeoService>();

// Add SearchService
builder.Services.AddSingleton<ISearchService, SearchService>();

builder.Services.AddSingleton<ICheckIpService, CheckIpService>();

// Add BackgroundService
builder.Services.AddHostedService<ClearCacheService>();

// Configure CORS
var frontendUrls = builder.Configuration.GetSection("FrontendUrl").Get<string[]>();
if (frontendUrls is null || frontendUrls.Length == 0)
{
    frontendUrls = ["*"];
}

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.WithOrigins(frontendUrls)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddCarter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Thêm middleware xử lý exception toàn cục
app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

// Use Cors
app.UseCors();

app.MapDefaultEndpoints();
app.MapCarter();

// Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.Run();