var builder = DistributedApplication.CreateBuilder(args);

// API service - serves REST endpoints for content
// Port 5001 defined in launchSettings.json
var api = builder.AddProject<Projects.TechHub_Api>("api");

// Web frontend - Blazor SSR app that consumes the API
// Port 5003 defined in launchSettings.json
_ = builder.AddProject<Projects.TechHub_Web>("web")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
