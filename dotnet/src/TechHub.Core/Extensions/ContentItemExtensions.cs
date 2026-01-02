using TechHub.Core.DTOs;
using TechHub.Core.Models;

namespace TechHub.Core.Extensions;

/// <summary>
/// Extension methods for ContentItem entity to DTO conversion
/// </summary>
public static class ContentItemExtensions
{
    /// <summary>
    /// Convert ContentItem entity to ContentItemDto (summary view)
    /// </summary>
    public static ContentItemDto ToDto(this ContentItem item, string sectionUrl)
    {
        return new ContentItemDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            Author = item.Author,
            DateEpoch = item.DateEpoch,
            DateIso = item.DateIso,
            Collection = item.Collection,
            AltCollection = item.AltCollection,
            Categories = item.Categories,
            Tags = item.Tags,
            Excerpt = item.Excerpt,
            ExternalUrl = item.ExternalUrl,
            VideoId = item.VideoId,
            Url = item.GetUrlInSection(sectionUrl)
        };
    }

    /// <summary>
    /// Convert ContentItem entity to ContentItemDetailDto (full detail view with rendered HTML)
    /// </summary>
    public static ContentItemDetailDto ToDetailDto(this ContentItem item, string sectionUrl)
    {
        return new ContentItemDetailDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            Author = item.Author,
            DateEpoch = item.DateEpoch,
            DateIso = item.DateIso,
            Collection = item.Collection,
            AltCollection = item.AltCollection,
            Categories = item.Categories,
            Tags = item.Tags,
            RenderedHtml = item.RenderedHtml,
            Excerpt = item.Excerpt,
            ExternalUrl = item.ExternalUrl,
            VideoId = item.VideoId,
            Url = item.GetUrlInSection(sectionUrl)
        };
    }

    /// <summary>
    /// Convert collection of ContentItem entities to DTOs
    /// </summary>
    public static IReadOnlyList<ContentItemDto> ToDtos(
        this IEnumerable<ContentItem> items,
        string sectionUrl)
    {
        return items.Select(item => item.ToDto(sectionUrl)).ToList();
    }
}
