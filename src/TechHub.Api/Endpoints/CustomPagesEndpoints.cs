using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Api.Endpoints;

/// <summary>
/// API endpoints for custom pages.
/// Custom page JSON is stored in the custom_page_data database table and managed
/// via the admin UI at /admin/custom-pages.
/// </summary>
public static class CustomPagesEndpoints
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static IEndpointRouteBuilder MapCustomPagesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/custom-pages")
            .WithTags("Custom Pages")
            .WithDescription("Endpoints for custom standalone pages");

        group.MapGet("/dx-space", GetDXSpaceData)
            .WithName("GetDXSpaceData")
            .WithSummary("Get DX Space page data")
            .WithDescription("Returns structured data for the Developer Experience Space custom page")
            .Produces<DXSpacePageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/handbook", GetHandbookData)
            .WithName("GetHandbookData")
            .WithSummary("Get GitHub Copilot Handbook page data")
            .WithDescription("Returns structured data for the GitHub Copilot Handbook custom page")
            .Produces<HandbookPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/levels", GetLevelsData)
            .WithName("GetLevelsData")
            .WithSummary("Get Levels of Enlightenment page data")
            .WithDescription("Returns structured data for the GitHub Copilot Levels of Enlightenment custom page")
            .Produces<LevelsPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/features", GetFeaturesData)
            .WithName("GetFeaturesData")
            .WithSummary("Get GitHub Copilot Features page data")
            .WithDescription("Returns structured data for the GitHub Copilot Features custom page")
            .Produces<FeaturesPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/genai-basics", GetGenAIBasicsData)
            .WithName("GetGenAIBasicsData")
            .WithSummary("Get GenAI Basics page data")
            .WithDescription("Returns structured data for the GenAI Basics custom page")
            .Produces<GenAIPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/genai-advanced", GetGenAIAdvancedData)
            .WithName("GetGenAIAdvancedData")
            .WithSummary("Get GenAI Advanced page data")
            .WithDescription("Returns structured data for the GenAI Advanced custom page")
            .Produces<GenAIPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/genai-applied", GetGenAIAppliedData)
            .WithName("GetGenAIAppliedData")
            .WithSummary("Get GenAI Applied page data")
            .WithDescription("Returns structured data for the GenAI Applied custom page")
            .Produces<GenAIPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/sdlc", GetSDLCData)
            .WithName("GetSDLCData")
            .WithSummary("Get AI SDLC page data")
            .WithDescription("Returns structured data for the AI SDLC custom page")
            .Produces<SDLCPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/tool-tips", GetToolTipsData)
            .WithName("GetToolTipsData")
            .WithSummary("Get GitHub Copilot Tool Tips page data")
            .WithDescription("Returns structured data for the GitHub Copilot Tool Tips custom page")
            .Produces<ToolTipsPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/getting-started", GetGettingStartedData)
            .WithName("GetGettingStartedData")
            .WithSummary("Get GitHub Copilot Getting Started page data")
            .WithDescription("Returns structured data for the GitHub Copilot Getting Started guide")
            .Produces<GettingStartedPageData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/hero-banner", GetHeroBannerData)
            .WithName("GetHeroBannerData")
            .WithSummary("Get hero banner data")
            .WithDescription("Returns all hero banner cards regardless of date; clients filter by startDate/endDate.")
            .Produces<HeroBannerData>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    // ── Generic helpers ──────────────────────────────────────────────────────

    private static async Task<Results<Ok<T>, NotFound>> GetPageData<T>(
        string key,
        ICustomPageDataRepository repo,
        CancellationToken ct)
    {
        var entry = await repo.GetByKeyAsync(key, ct);
        if (entry is null)
        {
            return TypedResults.NotFound();
        }

        var data = JsonSerializer.Deserialize<T>(entry.JsonData, _jsonOptions);
        return data is null ? TypedResults.NotFound() : TypedResults.Ok(data);
    }

    private static async Task<Results<Ok<T>, NotFound>> GetGenAIPageData<T>(
        string key,
        ICustomPageDataRepository repo,
        IMarkdownService markdownService,
        CancellationToken ct) where T : GenAIPageData
    {
        var entry = await repo.GetByKeyAsync(key, ct);
        if (entry is null)
        {
            return TypedResults.NotFound();
        }

        var data = JsonSerializer.Deserialize<T>(entry.JsonData, _jsonOptions);
        if (data is null)
        {
            return TypedResults.NotFound();
        }

        var processedSections = data.Sections.Select(section =>
        {
            var content = section.Content;

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

            var htmlContent = markdownService.RenderToHtml(content);

            if (section.Mermaid != null)
            {
                foreach (var diagram in section.Mermaid)
                {
                    var captionMarker = $"<p>{{{{CAPTION:{diagram.Id}}}}}</p>";
                    var caption = !string.IsNullOrWhiteSpace(diagram.Title)
                        ? $"<p class=\"mermaid-caption\">{WebUtility.HtmlEncode(diagram.Title)}</p>"
                        : string.Empty;
                    htmlContent = htmlContent.Replace(captionMarker, caption, StringComparison.Ordinal);
                }
            }

            return section with { Content = htmlContent };
        }).ToList();

        var json = JsonSerializer.Serialize(new { data.Title, data.Description, Sections = processedSections }, _jsonOptions);
        var processedData = JsonSerializer.Deserialize<T>(json, _jsonOptions)
            ?? throw new InvalidOperationException($"Failed to deserialize processed data for type {typeof(T).Name}");

        return TypedResults.Ok(processedData);
    }

    // ── Endpoint handlers ────────────────────────────────────────────────────

    private static Task<Results<Ok<DXSpacePageData>, NotFound>> GetDXSpaceData(
        ICustomPageDataRepository repo, CancellationToken ct)
        => GetPageData<DXSpacePageData>("dx-space", repo, ct);

    private static Task<Results<Ok<HandbookPageData>, NotFound>> GetHandbookData(
        ICustomPageDataRepository repo, CancellationToken ct)
        => GetPageData<HandbookPageData>("handbook", repo, ct);

    private static Task<Results<Ok<LevelsPageData>, NotFound>> GetLevelsData(
        ICustomPageDataRepository repo, CancellationToken ct)
        => GetPageData<LevelsPageData>("levels", repo, ct);

    private static Task<Results<Ok<FeaturesPageData>, NotFound>> GetFeaturesData(
        ICustomPageDataRepository repo, CancellationToken ct)
        => GetPageData<FeaturesPageData>("features", repo, ct);

    private static Task<Results<Ok<GenAIPageData>, NotFound>> GetGenAIBasicsData(
        ICustomPageDataRepository repo, IMarkdownService markdownService, CancellationToken ct)
        => GetGenAIPageData<GenAIPageData>("genai-basics", repo, markdownService, ct);

    private static Task<Results<Ok<GenAIPageData>, NotFound>> GetGenAIAdvancedData(
        ICustomPageDataRepository repo, IMarkdownService markdownService, CancellationToken ct)
        => GetGenAIPageData<GenAIPageData>("genai-advanced", repo, markdownService, ct);

    private static Task<Results<Ok<GenAIPageData>, NotFound>> GetGenAIAppliedData(
        ICustomPageDataRepository repo, IMarkdownService markdownService, CancellationToken ct)
        => GetGenAIPageData<GenAIPageData>("genai-applied", repo, markdownService, ct);

    private static Task<Results<Ok<SDLCPageData>, NotFound>> GetSDLCData(
        ICustomPageDataRepository repo, CancellationToken ct)
        => GetPageData<SDLCPageData>("sdlc", repo, ct);

    private static Task<Results<Ok<ToolTipsPageData>, NotFound>> GetToolTipsData(
        ICustomPageDataRepository repo, CancellationToken ct)
        => GetPageData<ToolTipsPageData>("tool-tips", repo, ct);

    private static Task<Results<Ok<HeroBannerData>, NotFound>> GetHeroBannerData(
        ICustomPageDataRepository repo, CancellationToken ct)
        => GetPageData<HeroBannerData>("hero-banner", repo, ct);

    private static async Task<Results<Ok<GettingStartedPageData>, NotFound>> GetGettingStartedData(
        ICustomPageDataRepository repo,
        IMarkdownService markdownService,
        CancellationToken ct)
    {
        var entry = await repo.GetByKeyAsync("getting-started", ct);
        if (entry is null)
        {
            return TypedResults.NotFound();
        }

        var data = JsonSerializer.Deserialize<GettingStartedPageData>(entry.JsonData, _jsonOptions);
        if (data is null)
        {
            return TypedResults.NotFound();
        }

        var processedSections = data.Sections
            .Select(s => s with { Content = markdownService.RenderToHtml(s.Content) })
            .ToList();

        return TypedResults.Ok(data with { Sections = processedSections });
    }
}