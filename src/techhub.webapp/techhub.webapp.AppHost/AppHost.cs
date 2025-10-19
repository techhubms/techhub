var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.techhub_webapp_ApiService>("apiservice");

builder.AddProject<Projects.techhub_webapp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
