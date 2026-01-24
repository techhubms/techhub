using TechHub.Core.DTOs;
using TechHub.Core.Models;

namespace TechHub.Core.Extensions;

/// <summary>
/// Extension methods for Section entity to DTO conversion
/// </summary>
public static class SectionExtensions
{
    /// <summary>
    /// Convert Section entity to SectionDto with display names
    /// </summary>
    public static SectionDto ToDto(this Section section, Dictionary<string, string> collectionDisplayNames)
    {
        ArgumentNullException.ThrowIfNull(section);
        ArgumentNullException.ThrowIfNull(collectionDisplayNames);

        return new SectionDto
        {
            Name = section.Name,
            Title = section.Title,
            Description = section.Description,
            Url = section.Url,
            Collections = [.. section.Collections.Select(c => c.ToDto(collectionDisplayNames))]
        };
    }

    /// <summary>
    /// Convert CollectionReference entity to CollectionReferenceDto with display name
    /// </summary>
    public static CollectionReferenceDto ToDto(this CollectionReference collection, Dictionary<string, string> displayNames)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(displayNames);

        // Look up display name from configuration, fallback to Title if not found
        var displayName = displayNames.TryGetValue(collection.Name.ToLowerInvariant(), out var name)
            ? name
            : collection.Title;

        return new CollectionReferenceDto
        {
            Name = collection.Name,
            Title = collection.Title,
            Url = collection.Url,
            Description = collection.Description,
            DisplayName = displayName,
            IsCustom = collection.IsCustom
        };
    }

    /// <summary>
    /// Convert collection of Section entities to DTOs with display names
    /// </summary>
    public static IReadOnlyList<SectionDto> ToDtos(this IEnumerable<Section> sections, Dictionary<string, string> collectionDisplayNames)
    {
        ArgumentNullException.ThrowIfNull(collectionDisplayNames);
        return [.. sections.Select(s => s.ToDto(collectionDisplayNames))];
    }
}
