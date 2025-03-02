var builder = DistributedApplication.CreateBuilder(args);
// Thêm Docker Api Sde
var apiSde = builder.AddDockerfile("apiSde", 
    "../","DockerfileGoApiSde")
    .WithImageTag("latest")
    .WithHttpEndpoint(8151,8151);
var api = builder.AddProject<Projects.Haihv_Elis_Tool_TraCuuGcn_Api>("api")
    .WaitFor(apiSde);
var app = builder.AddProject<Projects.Haihv_Elis_Tool_TraCuuGcn_Web_App>("app")
    .WithReference(api).WaitFor(api);

builder.Build().Run();