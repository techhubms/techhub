using TechHub.Core.Models;

namespace TechHub.Web.Services;

/// <summary>
/// In-memory cache of sections loaded at application startup.
/// Provides synchronous access to section data for immediate rendering without flicker.
/// </summary>
public class SectionCache
{
    private Dictionary<string, Section> _sectionsByName = [];

    public IReadOnlyList<Section> Sections { get; private set; } = [];

    public void Initialize(IReadOnlyList<Section> sections)
    {
        Sections = sections;
        _sectionsByName = sections.ToDictionary(s => s.Name, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Get a section by name from the cache. Case-insensitive lookup.
    /// </summary>
    /// <param name="sectionName">The section name to look up</param>
    /// <returns>The section DTO if found, otherwise null</returns>
    public Section? GetSectionByName(string sectionName)
    {
        _sectionsByName.TryGetValue(sectionName, out var section);
        return section;
    }
}
