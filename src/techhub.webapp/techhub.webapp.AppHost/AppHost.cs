var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.techhub_webapp_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.techhub_webapp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
