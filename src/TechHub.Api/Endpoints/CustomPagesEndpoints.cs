using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.DTOs;

namespace TechHub.Api.Endpoints;

/// <summary>
/// API endpoints for custom pages
/// </summary>
internal static class CustomPagesEndpoints
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Find solution root by walking up directory tree looking for TechHub.slnx
    /// </summary>
    private static string FindSolutionRoot(string startPath)
    {
        var directory = new DirectoryInfo(startPath);
        while (directory != null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "TechHub.slnx")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        // Fallback to start path if solution file not found
        return startPath;
    }

    /// <summary>
    /// Resolves the collections path from configuration, handling relative paths
    /// </summary>
    private static string ResolveCollectionsPath(string collectionsPath, string contentRootPath)
    {
        if (Path.IsPathRooted(collectionsPath))
        {
            return collectionsPath;
        }

        var solutionRoot = FindSolutionRoot(contentRootPath);
        return Path.Combine(solutionRoot, collectionsPath);
    }

    /// <summary>
    /// Maps all custom pages endpoints to the application
    /// </summary>
    public static IEndpointRouteBuilder MapCustomPagesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/custom-pages")
            .WithTags("Custom Pages")
            .WithDescription("Endpoints for custom standalone pages");

        // Get DX Space page data (specific endpoint with structured data)
        group.MapGet("/dx-space", GetDXSpaceData)
            .WithName("GetDXSpaceData")
            .WithSummary("Get DX Space page data")
            .WithDescription("Returns structured data for the Developer Experience Space custom page")
            .Produces<DXSpacePageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Get Handbook page data
        group.MapGet("/handbook", GetHandbookData)
            .WithName("GetHandbookData")
            .WithSummary("Get GitHub Copilot Handbook page data")
            .WithDescription("Returns structured data for the GitHub Copilot Handbook custom page")
            .Produces<HandbookPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Get Levels of Enlightenment page data
        group.MapGet("/levels", GetLevelsData)
            .WithName("GetLevelsData")
            .WithSummary("Get Levels of Enlightenment page data")
            .WithDescription("Returns structured data for the GitHub Copilot Levels of Enlightenment custom page")
            .Produces<LevelsPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Get VS Code Updates page data
        group.MapGet("/vscode-updates", GetVSCodeUpdatesData)
            .WithName("GetVSCodeUpdatesData")
            .WithSummary("Get VS Code Updates page data")
            .WithDescription("Returns structured data for the VS Code Updates custom page")
            .Produces<VSCodeUpdatesPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Get Features page data
        group.MapGet("/features", GetFeaturesData)
            .WithName("GetFeaturesData")
            .WithSummary("Get GitHub Copilot Features page data")
            .WithDescription("Returns structured data for the GitHub Copilot Features custom page")
            .Produces<FeaturesPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Get GenAI Basics page data
        group.MapGet("/genai-basics", GetGenAIBasicsData)
            .WithName("GetGenAIBasicsData")
            .WithSummary("Get GenAI Basics page data")
            .WithDescription("Returns structured data for the GenAI Basics custom page")
            .Produces<GenAIBasicsPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Get GenAI Advanced page data
        group.MapGet("/genai-advanced", GetGenAIAdvancedData)
            .WithName("GetGenAIAdvancedData")
            .WithSummary("Get GenAI Advanced page data")
            .WithDescription("Returns structured data for the GenAI Advanced custom page")
            .Produces<GenAIAdvancedPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Get GenAI Applied page data
        group.MapGet("/genai-applied", GetGenAIAppliedData)
            .WithName("GetGenAIAppliedData")
            .WithSummary("Get GenAI Applied page data")
            .WithDescription("Returns structured data for the GenAI Applied custom page")
            .Produces<GenAIAppliedPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Get SDLC page data
        group.MapGet("/sdlc", GetSDLCData)
            .WithName("GetSDLCData")
            .WithSummary("Get AI in the SDLC page data")
            .WithDescription("Returns structured data for the AI in the SDLC custom page")
            .Produces<SDLCPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    /// <summary>
    /// Generic helper to load and deserialize custom page JSON data
    /// </summary>
    private static async Task<Results<Ok<T>, NotFound>> GetPageData<T>(
        string jsonFileName,
        IWebHostEnvironment env,
        IOptions<AppSettings> settings,
        CancellationToken cancellationToken)
    {
        var collectionsPath = ResolveCollectionsPath(settings.Value.Content.CollectionsPath, env.ContentRootPath);
        var jsonPath = Path.Combine(collectionsPath, "_custom", jsonFileName);

        if (!File.Exists(jsonPath))
        {
            return TypedResults.NotFound();
        }

        var jsonContent = await File.ReadAllTextAsync(jsonPath, cancellationToken);
        var data = JsonSerializer.Deserialize<T>(jsonContent, _jsonOptions);

        if (data == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(data);
    }

    private static Task<Results<Ok<DXSpacePageData>, NotFound>> GetDXSpaceData(
        IWebHostEnvironment env, IOptions<AppSettings> settings, CancellationToken cancellationToken)
        => GetPageData<DXSpacePageData>("dx-space.json", env, settings, cancellationToken);

    private static Task<Results<Ok<HandbookPageData>, NotFound>> GetHandbookData(
        IWebHostEnvironment env, IOptions<AppSettings> settings, CancellationToken cancellationToken)
        => GetPageData<HandbookPageData>("handbook.json", env, settings, cancellationToken);

    private static Task<Results<Ok<LevelsPageData>, NotFound>> GetLevelsData(
        IWebHostEnvironment env, IOptions<AppSettings> settings, CancellationToken cancellationToken)
        => GetPageData<LevelsPageData>("levels.json", env, settings, cancellationToken);

    private static Task<Results<Ok<VSCodeUpdatesPageData>, NotFound>> GetVSCodeUpdatesData(
        IWebHostEnvironment env, IOptions<AppSettings> settings, CancellationToken cancellationToken)
        => GetPageData<VSCodeUpdatesPageData>("vscode-updates.json", env, settings, cancellationToken);

    private static Task<Results<Ok<FeaturesPageData>, NotFound>> GetFeaturesData(
        IWebHostEnvironment env, IOptions<AppSettings> settings, CancellationToken cancellationToken)
        => GetPageData<FeaturesPageData>("features.json", env, settings, cancellationToken);

    private static Task<Results<Ok<GenAIBasicsPageData>, NotFound>> GetGenAIBasicsData(
        IWebHostEnvironment env, IOptions<AppSettings> settings, CancellationToken cancellationToken)
        => GetPageData<GenAIBasicsPageData>("genai-basics.json", env, settings, cancellationToken);

    private static Task<Results<Ok<GenAIAdvancedPageData>, NotFound>> GetGenAIAdvancedData(
        IWebHostEnvironment env, IOptions<AppSettings> settings, CancellationToken cancellationToken)
        => GetPageData<GenAIAdvancedPageData>("genai-advanced.json", env, settings, cancellationToken);

    private static Task<Results<Ok<GenAIAppliedPageData>, NotFound>> GetGenAIAppliedData(
        IWebHostEnvironment env, IOptions<AppSettings> settings, CancellationToken cancellationToken)
        => GetPageData<GenAIAppliedPageData>("genai-applied.json", env, settings, cancellationToken);

    private static Task<Results<Ok<SDLCPageData>, NotFound>> GetSDLCData(
        IWebHostEnvironment env, IOptions<AppSettings> settings, CancellationToken cancellationToken)
        => GetPageData<SDLCPageData>("sdlc.json", env, settings, cancellationToken);
}
