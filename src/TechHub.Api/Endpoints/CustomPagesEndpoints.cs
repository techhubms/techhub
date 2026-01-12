using Microsoft.AspNetCore.Http.HttpResults;
using TechHub.Core.DTOs;
using TechHub.Core.Interfaces;

namespace TechHub.Api.Endpoints;

/// <summary>
/// API endpoints for custom pages
/// </summary>
internal static class CustomPagesEndpoints
{
    /// <summary>
    /// Maps all custom pages endpoints to the application
    /// </summary>
    public static IEndpointRouteBuilder MapCustomPagesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/custom-pages")
            .WithTags("Custom Pages")
            .WithDescription("Endpoints for custom standalone pages");

        // Get all custom pages
        group.MapGet("", GetAllCustomPages)
            .WithName("GetAllCustomPages")
            .WithSummary("Get all custom pages")
            .WithDescription("Returns list of all custom pages with basic metadata")
            .Produces<IEnumerable<CustomPageDto>>(StatusCodes.Status200OK);

        // Get specific custom page by slug
        group.MapGet("/{slug}", GetCustomPageBySlug)
            .WithName("GetCustomPageBySlug")
            .WithSummary("Get custom page by slug")
            .WithDescription("Returns detailed custom page content by slug")
            .Produces<CustomPageDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    private static async Task<Ok<IEnumerable<CustomPageDto>>> GetAllCustomPages(
        ICustomPageRepository customPageRepository,
        CancellationToken cancellationToken)
    {
        var pages = await customPageRepository.GetAllAsync(cancellationToken);

        var dtos = pages.Select(page => new CustomPageDto
        {
            Slug = page.Slug,
            Title = page.Title,
            Description = page.Description,
            Url = page.Permalink,
            Categories = page.Categories
        });

        return TypedResults.Ok(dtos);
    }

    private static async Task<Results<Ok<CustomPageDetailDto>, NotFound>> GetCustomPageBySlug(
        string slug,
        ICustomPageRepository customPageRepository,
        IMarkdownService markdownService,
        CancellationToken cancellationToken)
    {
        var page = await customPageRepository.GetBySlugAsync(slug, cancellationToken);

        if (page == null)
        {
            return TypedResults.NotFound();
        }

        var renderedHtml = markdownService.RenderToHtml(page.Content);

        var dto = new CustomPageDetailDto
        {
            Slug = page.Slug,
            Title = page.Title,
            Description = page.Description,
            Url = page.Permalink,
            Categories = page.Categories,
            RenderedHtml = renderedHtml,
            SidebarInfo = page.SidebarInfo
        };

        return TypedResults.Ok(dto);
    }
}
