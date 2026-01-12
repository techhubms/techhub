using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Services;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Service for mapping between section names, URLs, and display titles.
/// Reads from appsettings.json configuration as single source of truth.
/// </summary>
public class SectionMappingService : ISectionMappingService
{
    private readonly Dictionary<string, string> _urlToTitle;
    private readonly Dictionary<string, string> _nameToUrl;

    public SectionMappingService(IOptions<AppSettings> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var sections = options.Value.Content.Sections;

        // Build URL → Title mapping (using section key as URL identifier)
        _urlToTitle = sections.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Title,
            StringComparer.OrdinalIgnoreCase);

        // Build Name → URL mapping (section name is the key itself)
        _nameToUrl = sections.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Key,
            StringComparer.OrdinalIgnoreCase);
    }

    public string GetSectionTitle(string sectionUrl)
    {
        if (string.IsNullOrWhiteSpace(sectionUrl))
            return sectionUrl ?? string.Empty;

        return _urlToTitle.TryGetValue(sectionUrl, out var title)
            ? title
            : System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(sectionUrl.ToLowerInvariant());
    }

    public string GetSectionUrl(string sectionName)
    {
        if (string.IsNullOrWhiteSpace(sectionName))
            return sectionName ?? string.Empty;

        return _nameToUrl.TryGetValue(sectionName, out var url)
            ? url
            : sectionName.ToLowerInvariant().Replace(" ", "-", StringComparison.Ordinal);
    }

    public string GetCollectionDisplayName(string collectionName)
    {
        if (string.IsNullOrWhiteSpace(collectionName))
            return collectionName ?? string.Empty;

        return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(collectionName.ToLowerInvariant());
    }
}
