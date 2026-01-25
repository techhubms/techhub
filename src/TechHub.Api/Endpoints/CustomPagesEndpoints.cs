using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.DTOs;
using TechHub.Core.Interfaces;

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
            .Produces<GenAIPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Get GenAI Advanced page data
        group.MapGet("/genai-advanced", GetGenAIAdvancedData)
            .WithName("GetGenAIAdvancedData")
            .WithSummary("Get GenAI Advanced page data")
            .WithDescription("Returns structured data for the GenAI Advanced custom page")
            .Produces<GenAIPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Get GenAI Applied page data
        group.MapGet("/genai-applied", GetGenAIAppliedData)
            .WithName("GetGenAIAppliedData")
            .WithSummary("Get GenAI Applied page data")
            .WithDescription("Returns structured data for the GenAI Applied custom page")
            .Produces<GenAIPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // Get SDLC page data
        group.MapGet("/sdlc", GetSDLCData)
            .WithName("GetSDLCData")
            .WithSummary("Get AI SDLC page data")
            .WithDescription("Returns structured data for the AI SDLC custom page")
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

    /// <summary>
    /// Special handler for GenAI pages that process markdown content.
    /// Replaces {{mermaid:id}} placeholders with actual diagram code and renders markdown to HTML.
    /// </summary>
    private static async Task<Results<Ok<T>, NotFound>> GetGenAIPageData<T>(
        string jsonFileName,
        IWebHostEnvironment env,
        IOptions<AppSettings> settings,
        IMarkdownService markdownService,
        CancellationToken cancellationToken) where T : GenAIPageData
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

        // Process each section: replace mermaid placeholders and render markdown
        var processedSections = data.Sections.Select(section =>
        {
            var content = section.Content;

            // Replace {{mermaid:id}} placeholders with diagram code + unique caption marker
            if (section.Mermaid != null)
            {
                foreach (var diagram in section.Mermaid)
                {
                    var placeholder = $"{{{{mermaid:{diagram.Id}}}}}";
                    var captionMarker = $"{{{{CAPTION:{diagram.Id}}}}}";
                    var mermaidBlock = $"```mermaid\n{diagram.Diagram}\n```\n\n{captionMarker}";
                    content = content.Replace(placeholder, mermaidBlock, StringComparison.Ordinal);
                }
            }

            // Render markdown to HTML
            var htmlContent = markdownService.RenderToHtml(content);

            // Replace caption markers with actual caption paragraphs (only if title exists)
            if (section.Mermaid != null)
            {
                foreach (var diagram in section.Mermaid)
                {
                    var captionMarker = $"<p>{{{{CAPTION:{diagram.Id}}}}}</p>";
                    // Only add caption if title is provided
                    var caption = !string.IsNullOrWhiteSpace(diagram.Title)
                        ? $"<p class=\"mermaid-caption\">{System.Net.WebUtility.HtmlEncode(diagram.Title)}</p>"
                        : string.Empty;
                    htmlContent = htmlContent.Replace(captionMarker, caption, StringComparison.Ordinal);
                }
            }

            return section with { Content = htmlContent };
        }).ToList();

        // Serialize and deserialize to create a new instance with updated sections
        // This approach works because T is a record type implementing IGenAIPageData
        var json = JsonSerializer.Serialize(new
        {
            data.Title,
            data.Description,
            Sections = processedSections
        }, _jsonOptions);

        var processedData = JsonSerializer.Deserialize<T>(json, _jsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize processed data for type {typeof(T).Name}");

        return TypedResults.Ok(processedData);
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

    private static Task<Results<Ok<GenAIPageData>, NotFound>> GetGenAIBasicsData(
        IWebHostEnvironment env,
        IOptions<AppSettings> settings,
        IMarkdownService markdownService,
        CancellationToken cancellationToken)
        => GetGenAIPageData<GenAIPageData>("genai-basics.json", env, settings, markdownService, cancellationToken);

    private static Task<Results<Ok<GenAIPageData>, NotFound>> GetGenAIAdvancedData(
        IWebHostEnvironment env,
        IOptions<AppSettings> settings,
        IMarkdownService markdownService,
        CancellationToken cancellationToken)
        => GetGenAIPageData<GenAIPageData>("genai-advanced.json", env, settings, markdownService, cancellationToken);

    private static Task<Results<Ok<GenAIPageData>, NotFound>> GetGenAIAppliedData(
        IWebHostEnvironment env,
        IOptions<AppSettings> settings,
        IMarkdownService markdownService,
        CancellationToken cancellationToken)
        => GetGenAIPageData<GenAIPageData>("genai-applied.json", env, settings, markdownService, cancellationToken);

    private static Task<Results<Ok<SDLCPageData>, NotFound>> GetSDLCData(
        IWebHostEnvironment env, IOptions<AppSettings> settings, CancellationToken cancellationToken)
        => GetPageData<SDLCPageData>("sdlc.json", env, settings, cancellationToken);
}
