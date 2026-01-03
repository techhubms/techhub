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
        
        // Convert configuration to Section models at construction time
        _sections = settings.Value.Content.Sections
            .Select(kvp => ConvertToSection(kvp.Key, kvp.Value))
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
    /// Get section by ID
    /// </summary>
    public Task<Section?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var section = _sections.FirstOrDefault(s => s.Id == id);
        return Task.FromResult(section);
    }

    /// <summary>
    /// Convert SectionConfig from appsettings.json to Section model
    /// </summary>
    private static Section ConvertToSection(string sectionId, SectionConfig config)
    {
        // Map collection configurations to CollectionReference models
        // Filter out custom pages (those without a collection field) for now
        var collections = config.Collections
            .Where(c => !string.IsNullOrEmpty(c.Collection))
            .Select(c => new CollectionReference
            {
                Title = c.Title,
                Collection = c.Collection!,
                Url = c.Url,
                Description = c.Description,
                IsCustom = c.Custom
            })
            .ToList();

        return new Section
        {
            Id = sectionId,
            Title = config.Title,
            Description = config.Description,
            Url = config.Url,
            BackgroundImage = config.Image,
            Category = config.Category,
            Collections = collections
        };
    }
}
