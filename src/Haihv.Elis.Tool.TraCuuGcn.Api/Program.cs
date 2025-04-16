using System.Text;
using Carter;
using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
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

// Configure Redis
var redisConnectionString = builder.Configuration["Redis:ConnectionString"];
// Add Caching
builder.Services.AddCache(redisConnectionString);

// Add Jwt
builder.Services.AddSingleton(
    _ => new TokenProvider(builder.Configuration["Jwt:SecretKey"]!,
        builder.Configuration["Jwt:Issuer"]!,
        builder.Configuration["Jwt:Audience"]!,
        builder.Configuration.GetValue<int>("Jwt:ExpireMinutes")));

// // Add service Authentication and Authorization for Identity Server
// builder.Services.AddAuthorizationBuilder();
//
// // Cấu hình Authentication với nhiều JwtScheme
// var jwtTokenSettings = new JwtTokenSettings();
// builder.Configuration.GetSection("JwtTokenSettings").Bind(jwtTokenSettings);
// builder.Services.AddAuthentication(options =>
//     {
//         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//     })
//     .AddJwtBearer(options =>
//     {
//         options.RequireHttpsMetadata = false;
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             IssuerSigningKeys = jwtTokenSettings.SecretKeys.Select(key =>
//                 new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))).ToList(),
//             ValidIssuers = jwtTokenSettings.Issuers,
//             ValidAudiences = jwtTokenSettings.Audiences,
//             ClockSkew = TimeSpan.Zero,
//         };
//     });
var keycloakConfig = builder.Configuration.GetSection("OpenIdConnect");
var authority = keycloakConfig["Authority"];
var audience = keycloakConfig["Audience"];

builder.Services.AddAuthentication()
    .AddJwtBearer("Bearer", jwtOptions =>
    {
        jwtOptions.Authority = authority;
        jwtOptions.Audience = audience;
        jwtOptions.RequireHttpsMetadata = true;
    });
builder.Services.AddAuthorization();
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

// Add GeoService
builder.Services.AddSingleton<IGeoService, GeoService>();

// Add SearchService
builder.Services.AddSingleton<ISearchService, SearchService>();

// Add AuthenticationService
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
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
app.MapGroup("/api").MapCarter();

// Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.Run();