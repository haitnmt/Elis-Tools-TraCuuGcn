var builder = DistributedApplication.CreateBuilder(args);
var cache = builder.AddValkey("Cache", 6379);
var sdeApi = builder.AddGolangApp("sdeApi", "../Haihv.Elis.Tool.TraCuuGcn.Api.Sde")
    .WithHttpEndpoint(8151, 8151, "localhost", isProxied: false);
var api = builder.AddProject<Projects.Haihv_Elis_Tool_TraCuuGcn_Api>("TraCuuGcn-api")
    .WithReference(sdeApi)
    .WithReference(cache)
    .WaitFor(cache)
    .WaitFor(sdeApi);
builder.AddProject<Projects.Haihv_Elis_Tool_TraCuuGcn_Web_App>("TraCuuGcn-app")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();