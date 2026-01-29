using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Repository for loading sections from appsettings.json configuration
/// Reads sections directly from IOptions instead of file system
/// </summary>
public sealed class ConfigurationBasedSectionRepository : ISectionRepository
{
    private readonly IReadOnlyList<Section> _sections;

    public ConfigurationBasedSectionRepository(IOptions<AppSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        // Get collection display names mapping
        var collectionDisplayNames = settings.Value.Content.CollectionDisplayNames;

        // Define section display order (matches live site - starts with "all")
        var sectionOrder = new[]
        {
            "all", "github-copilot", "ai", "ml", "devops", "azure", "coding", "security"
        };

        // Convert configuration to Section models and apply ordering
        var sectionsDict = settings.Value.Content.Sections
            .Select(kvp => ConvertToSection(kvp.Key, kvp.Value, collectionDisplayNames))
            .ToDictionary(s => s.Name);

        // Order sections according to defined order, then any remaining alphabetically
        _sections = sectionOrder
            .Where(name => sectionsDict.ContainsKey(name))
            .Select(name => sectionsDict[name])
            .Concat(sectionsDict.Values.Where(s => !sectionOrder.Contains(s.Name)).OrderBy(s => s.Title))
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// Get all sections from configuration
    /// Data is pre-loaded and cached in memory from constructor
    /// </summary>
    public Task<IReadOnlyList<Section>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_sections);
    }

    /// <summary>
    /// Get section by name
    /// </summary>
    public Task<Section?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);

        var section = _sections.FirstOrDefault(s => s.Name == name);
        return Task.FromResult(section);
    }

    /// <summary>
    /// Convert SectionConfig from appsettings.json to Section model
    /// </summary>
    private static Section ConvertToSection(string sectionName, SectionConfig config, Dictionary<string, string> collectionDisplayNames)
    {
        // Map collection configurations to Collection models
        // The key in the Collections dictionary is the collection name
        var collections = config.Collections
            .Select(kvp =>
            {
                // Look up display name from global config, fallback to collection title
                var displayName = collectionDisplayNames.TryGetValue(kvp.Key.ToLowerInvariant(), out var name)
                    ? name
                    : kvp.Value.Title;
                return new Collection(kvp.Key, kvp.Value.Title, kvp.Value.Url, kvp.Value.Description, displayName, kvp.Value.Custom);
            })
            .ToList();

        return new Section(sectionName, config.Title, config.Description, config.Url, collections);
    }
}
