using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Services.RoundupGeneration;

/// <summary>
/// Registers all roundup generation services, including internal step classes
/// that are not directly accessible from other assemblies.
/// </summary>
public static class RoundupGenerationExtensions
{
    /// <summary>
    /// Adds the roundup generation pipeline services to the service collection.
    /// Must be called after <see cref="RoundupGeneratorOptions"/> is configured.
    /// </summary>
    public static IServiceCollection AddRoundupGeneration(this IServiceCollection services)
    {
        // Unwrapped options for internal step classes
        services.AddScoped(sp =>
            sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<RoundupGeneratorOptions>>().Value);

        // Internal step classes
        services.AddScoped<RoundupAiHelper>();
        services.AddScoped<RoundupRelevanceFilter>();
        services.AddScoped<RoundupNewsWriter>();
        services.AddScoped<RoundupNarrativeEnhancer>();
        services.AddScoped<RoundupCondenser>();
        services.AddScoped<RoundupMetadataGenerator>();

        // Orchestrator
        services.AddScoped<IRoundupGeneratorService, RoundupGeneratorService>();

        return services;
    }
}
