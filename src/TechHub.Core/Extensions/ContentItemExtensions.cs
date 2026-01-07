using TechHub.Core.DTOs;
using TechHub.Core.Models;
using TechHub.Core.Helpers;

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
        ArgumentNullException.ThrowIfNull(item);

        // Determine primary section based on categories and collection
        var primarySectionUrl = SectionPriorityHelper.GetPrimarySectionUrl(item.Categories, item.CollectionName);
        var primarySectionName = SectionPriorityHelper.GetPrimarySectionName(item.Categories, item.CollectionName);

        return new ContentItemDto
        {
            Slug = item.Slug,
            Title = item.Title,
            Description = item.Description,
            Author = item.Author,
            DateEpoch = item.DateEpoch,
            DateIso = item.DateIso,
            CollectionName = item.CollectionName,
            AltCollection = item.AltCollection,
            Categories = item.Categories,
            PrimarySection = primarySectionName,
            Tags = item.Tags,
            Excerpt = item.Excerpt,
            ExternalUrl = item.ExternalUrl,
            VideoId = item.VideoId,
            ViewingMode = item.ViewingMode,
            Url = item.GetUrlInSection(sectionUrl)
        };
    }

    /// <summary>
    /// Convert ContentItem entity to ContentItemDetailDto (full detail view with rendered HTML)
    /// </summary>
    public static ContentItemDetailDto ToDetailDto(this ContentItem item, string sectionUrl)
    {
        ArgumentNullException.ThrowIfNull(item);

        // Determine primary section based on categories and collection
        var primarySectionUrl = SectionPriorityHelper.GetPrimarySectionUrl(item.Categories, item.CollectionName);
        var primarySectionName = SectionPriorityHelper.GetPrimarySectionName(item.Categories, item.CollectionName);

        return new ContentItemDetailDto
        {
            Slug = item.Slug,
            Title = item.Title,
            Description = item.Description,
            Author = item.Author,
            DateEpoch = item.DateEpoch,
            DateIso = item.DateIso,
            CollectionName = item.CollectionName,
            AltCollection = item.AltCollection,
            Categories = item.Categories,
            PrimarySection = primarySectionName,
            Tags = item.Tags,
            RenderedHtml = item.RenderedHtml,
            Excerpt = item.Excerpt,
            ExternalUrl = item.ExternalUrl,
            VideoId = item.VideoId,
            ViewingMode = item.ViewingMode,
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
