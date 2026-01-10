using TechHub.Core.DTOs;

namespace TechHub.Web.Services;

/// <summary>
/// In-memory cache of sections loaded at application startup.
/// Provides synchronous access to section data for immediate rendering without flicker.
/// </summary>
internal class SectionCache
{
    public IReadOnlyList<SectionDto> Sections { get; private set; } = [];

    public void Initialize(IReadOnlyList<SectionDto> sections)
    {
        Sections = sections;
    }
}
