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

        // Define section display order (matches live site - starts with "all")
        var sectionOrder = new[]
        {
            "all", "github-copilot", "ai", "ml", "devops", "azure", "coding", "security"
        };

        // Convert configuration to Section models and apply ordering
        var sectionsDict = settings.Value.Content.Sections
            .Select(kvp => ConvertToSection(kvp.Key, kvp.Value))
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
    /// Initialize the repository - no-op for configuration-based repository
    /// Data is already loaded from IOptions in constructor
    /// </summary>
    public Task<IReadOnlyList<Section>> InitializeAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_sections);
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
        var section = _sections.FirstOrDefault(s => s.Name == name);
        return Task.FromResult(section);
    }

    /// <summary>
    /// Convert SectionConfig from appsettings.json to Section model
    /// </summary>
    private static Section ConvertToSection(string sectionId, SectionConfig config)
    {
        // Map collection configurations to CollectionReference models
        // Include both regular collections AND custom pages
        var collections = config.Collections
            .Select(c => new CollectionReference
            {
                Title = c.Title,
                Name = c.Collection ?? "", // Empty string for custom pages (no collection field)
                Url = c.Url,
                Description = c.Description,
                IsCustom = string.IsNullOrEmpty(c.Collection) // Custom pages have no collection field
            })
            .ToList();

        return new Section
        {
            Name = sectionId,
            Title = config.Title,
            Description = config.Description,
            Url = config.Url,
            BackgroundImage = config.Image,
            Category = config.Category,
            Collections = collections
        };
    }
}
