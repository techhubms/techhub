namespace TechHub.AppHost;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        // Get the environment from parent process (set by Run function)
        // Defaults to "Development" if not specified
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var dotnetEnv = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "NOT_SET";

        // Log environment for verification - shows both env vars to diagnose issues
        Console.WriteLine($"🎯 TechHub.AppHost environment check:");
        Console.WriteLine($"   ASPNETCORE_ENVIRONMENT = {environment}");
        Console.WriteLine($"   DOTNET_ENVIRONMENT = {dotnetEnv}");
        Console.WriteLine($"   Configuring child services with: {environment}");

        // API service - serves REST endpoints for content
        // Port 5001 defined in launchSettings.json
        var api = builder.AddProject<Projects.TechHub_Api>("api")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", environment)
            .WithEnvironment("DOTNET_ENVIRONMENT", environment);

        // Web frontend - Blazor SSR app that consumes the API
        // Port 5003 defined in launchSettings.json
        builder.AddProject<Projects.TechHub_Web>("web")
            .WithReference(api)
            .WaitFor(api)
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", environment)
            .WithEnvironment("DOTNET_ENVIRONMENT", environment);

        builder.Build().Run();
    }
}
