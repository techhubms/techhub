using TechHub.Core.DTOs;

namespace TechHub.Web.Services;

/// <summary>
/// In-memory cache of sections loaded at application startup.
/// Provides synchronous access to section data for immediate rendering without flicker.
/// </summary>
public class SectionCache
{
    private IReadOnlyList<SectionDto> _sections = Array.Empty<SectionDto>();

    public IReadOnlyList<SectionDto> Sections => _sections;

    public void Initialize(IReadOnlyList<SectionDto> sections)
    {
        _sections = sections;
    }
}
