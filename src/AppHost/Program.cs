var builder = DistributedApplication.CreateBuilder(args);
// Thêm Docker Api Sde
// var apiSde = builder.AddDockerfile("apiSde", 
//     "../","DockerfileGoApiSde")
//     .WithImageTag("latest")
//     .WithHttpEndpoint(8151,8151);
// var apiLdap = builder.AddContainer("apiLdap",
//         "haitnmt/ldap-api", "latest")
//     .WithHttpsEndpoint(7271, 8080);
var api = builder.AddProject<Projects.Haihv_Elis_Tool_TraCuuGcn_Api>("api");
builder.AddProject<Projects.Haihv_Elis_Tool_TraCuuGcn_Web_App>("app")
    .WaitFor(api);

builder.Build().Run();