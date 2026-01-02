using TechHub.Core.DTOs;
using TechHub.Core.Models;

namespace TechHub.Core.Extensions;

/// <summary>
/// Extension methods for Section entity to DTO conversion
/// </summary>
public static class SectionExtensions
{
    /// <summary>
    /// Convert Section entity to SectionDto
    /// </summary>
    public static SectionDto ToDto(this Section section)
    {
        return new SectionDto
        {
            Id = section.Id,
            Title = section.Title,
            Description = section.Description,
            Url = section.Url,
            Category = section.Category,
            BackgroundImage = section.BackgroundImage,
            Collections = section.Collections.Select(c => c.ToDto()).ToList()
        };
    }

    /// <summary>
    /// Convert CollectionReference entity to CollectionReferenceDto
    /// </summary>
    public static CollectionReferenceDto ToDto(this CollectionReference collection)
    {
        return new CollectionReferenceDto
        {
            Title = collection.Title,
            Collection = collection.Collection,
            Url = collection.Url,
            Description = collection.Description,
            IsCustom = collection.IsCustom
        };
    }

    /// <summary>
    /// Convert collection of Section entities to DTOs
    /// </summary>
    public static IReadOnlyList<SectionDto> ToDtos(this IEnumerable<Section> sections)
    {
        return sections.Select(s => s.ToDto()).ToList();
    }
}
