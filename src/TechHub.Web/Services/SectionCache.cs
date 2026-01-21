using TechHub.Core.DTOs;

namespace TechHub.Web.Services;

/// <summary>
/// In-memory cache of sections loaded at application startup.
/// Provides synchronous access to section data for immediate rendering without flicker.
/// </summary>
internal class SectionCache
{
    private Dictionary<string, SectionDto> _sectionsByName = [];

    public IReadOnlyList<SectionDto> Sections { get; private set; } = [];

    public void Initialize(IReadOnlyList<SectionDto> sections)
    {
        Sections = sections;
        _sectionsByName = sections.ToDictionary(s => s.Name, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Get a section by name from the cache. Case-insensitive lookup.
    /// </summary>
    /// <param name="sectionName">The section name to look up</param>
    /// <returns>The section DTO if found, otherwise null</returns>
    public SectionDto? GetSectionByName(string sectionName)
    {
        _sectionsByName.TryGetValue(sectionName, out var section);
        return section;
    }
}
