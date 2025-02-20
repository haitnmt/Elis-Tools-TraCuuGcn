using System.Text;
using Haihv.Elis.Tool.TraCuuGcn.Api.Authenticate;
using Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;
using Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;
using Haihv.Elis.Tool.TraCuuGcn.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

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

// Add service Authentication and Authorization for Identity Server
// Cấu hình Authentication với nhiều JwtScheme
const string otherJwtScheme = "ApiIdVpdk";
var jwtApiIdVpdkSection = builder.Configuration.GetSection("JwtApiIdVpdk");
builder.Services.AddAuthorizationBuilder();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKeys = new List<SecurityKey>
            {
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtApiIdVpdkSection["SecretKey"]!))
            },
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
            // Chấp nhận nhiều Issuer
            ValidIssuers =
            [
                builder.Configuration["Jwt:Issuer"],
                jwtApiIdVpdkSection["Issuer"]
            ],
            ValidAudiences =
            [
                builder.Configuration["Jwt:Audience"],
                jwtApiIdVpdkSection["Audience"]
            ],
            ClockSkew = TimeSpan.Zero,
        };
    });
// Add ConnectionElisData
builder.Services.AddSingleton<IConnectionElisData, ConnectionElisData>();

// Add GcnQrService
builder.Services.AddSingleton<IGcnQrService, GcnQrService>();
// Add GiayChungNhanService
builder.Services.AddSingleton<IGiayChungNhanService, GiayChungNhanService>();
// Add ChuSuDungService
builder.Services.AddSingleton<IChuSuDungService, ChuSuDungService>();
// Add ThuaDatService
builder.Services.AddSingleton<IThuaDatService, ThuaDatService>();
// Add GeoService
builder.Services.AddSingleton<IGeoService, GeoService>();

// Add SearchService
builder.Services.AddSingleton<ISearchService, SearchService>();

// Add AuthenticationService
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();


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

#pragma warning disable SYSLIB0014
System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;
#pragma warning restore SYSLIB0014

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Use Cors
app.UseCors();

// Use Middleware
app.MapGiayChungNhanEndpoints();
app.MapChuSuDungEndpoints();
app.MapAuthenticationEndpoints();
app.MapAppSettingsEndpoints();
app.MapGeoEndPoints();
app.MapSearchEndpoints();

// Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.Run();