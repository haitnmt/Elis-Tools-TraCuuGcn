var builder = DistributedApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var cacheName = configuration["Aspire:CacheName"] ??  "cache";
var cache  = builder.AddValkey(cacheName,  6379);
var api = builder.AddProject<Projects.Haihv_Elis_Tool_TraCuuGcn_Api>("TraCuuGcn-api")
    .WithReference(cache)
    .WaitFor(cache);
builder.AddProject<Projects.Haihv_Elis_Tool_TraCuuGcn_Web_App>("TraCuuGcn-app")
    .WithReference(cache)
    .WithReference(api)
    .WaitFor(cache)
    .WaitFor(api);

builder.Build().Run();