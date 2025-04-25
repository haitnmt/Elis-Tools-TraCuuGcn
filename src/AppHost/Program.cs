var builder = DistributedApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var cacheName = configuration["Aspire:CacheName"] ??  "cache";
var cache  = builder.AddValkey(cacheName,  6379);
var sdeApi = builder.AddGolangApp("sdeApi", "../Haihv.Elis.Tool.TraCuuGcn.Api.Sde")
    .WithHttpEndpoint(8151, 8151, "localhost", isProxied: false);
var api = builder.AddProject<Projects.Haihv_Elis_Tool_TraCuuGcn_Api>("TraCuuGcn-api")
    .WithReference(sdeApi)
    .WithReference(cache)
    .WaitFor(sdeApi)
    .WaitFor(cache);
builder.AddProject<Projects.Haihv_Elis_Tool_TraCuuGcn_Web_App>("TraCuuGcn-app")
    .WithReference(cache)
    .WithReference(api)
    .WaitFor(cache)
    .WaitFor(api);

builder.Build().Run();