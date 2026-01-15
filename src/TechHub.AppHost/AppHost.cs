var builder = DistributedApplication.CreateBuilder(args);

// API service - serves REST endpoints for content
var api = builder.AddProject<Projects.TechHub_Api>("api")
    .WithExternalHttpEndpoints();

// Web frontend - Blazor SSR app that consumes the API
_ = builder.AddProject<Projects.TechHub_Web>("web")
    .WithExternalHttpEndpoints()
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
