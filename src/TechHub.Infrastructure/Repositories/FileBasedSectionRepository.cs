using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;
using TechHub.Core.Models;

namespace TechHub.Infrastructure.Repositories;

/// <summary>
/// Repository for loading sections from _data/sections.json
/// Reads the single source of truth for site structure
/// </summary>
public class FileBasedSectionRepository : ISectionRepository
{
    private readonly string _sectionsFilePath;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Internal DTO for deserializing sections.json which has different property names
    /// </summary>
    private class SectionJsonDto
    {
        [JsonPropertyName("title")]
        public required string Title { get; set; }
        
        [JsonPropertyName("description")]
        public required string Description { get; set; }
        
        [JsonPropertyName("url")]
        public required string Url { get; set; }
        
        [JsonPropertyName("section")]
        public required string Section { get; set; }
        
        [JsonPropertyName("image")]
        public required string Image { get; set; }
        
        [JsonPropertyName("category")]
        public required string Category { get; set; }
        
        [JsonPropertyName("collections")]
        public required List<CollectionJsonDto> Collections { get; set; }
    }

    /// <summary>
    /// Internal DTO for deserializing collection references from sections.json
    /// </summary>
    private class CollectionJsonDto
    {
        [JsonPropertyName("title")]
        public required string Title { get; set; }
        
        [JsonPropertyName("collection")]
        public string? Collection { get; set; } // Nullable for custom pages that don't have a collection
        
        [JsonPropertyName("url")]
        public required string Url { get; set; }
        
        [JsonPropertyName("description")]
        public required string Description { get; set; }
        
        [JsonPropertyName("custom")]
        public bool IsCustom { get; set; }
    }

    public FileBasedSectionRepository(IOptions<AppSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        var sectionsPath = settings.Value.Content.SectionsConfigPath;
        _sectionsFilePath = sectionsPath;
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };
    }

    /// <summary>
    /// Get all sections from sections.json
    /// </summary>
    public async Task<IReadOnlyList<Section>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_sectionsFilePath))
        {
            throw new FileNotFoundException($"sections.json not found at {_sectionsFilePath}");
        }

        var json = await File.ReadAllTextAsync(_sectionsFilePath, cancellationToken);
        
        // sections.json structure: Dictionary with section IDs as keys
        var sectionsDict = JsonSerializer.Deserialize<Dictionary<string, SectionJsonDto>>(json, _jsonOptions);
        
        if (sectionsDict == null || sectionsDict.Count == 0)
        {
            return new List<Section>();
        }

        // Convert DTOs to Section models
        var sections = sectionsDict.Select(kvp =>
        {
            var sectionName = kvp.Key;
            var dto = kvp.Value;
            
            // Map collection DTOs to CollectionReference models
            // Filter out custom pages (those without a collection field) for now
            var collections = dto.Collections
                .Where(c => !string.IsNullOrEmpty(c.Collection)) // Only include items with a collection field
                .Select(c => new CollectionReference
                {
                    Title = c.Title,
                    Collection = c.Collection!,
                    Url = c.Url,
                    Description = c.Description,
                    IsCustom = c.IsCustom
                })
                .ToList();
            
            return new Section
            {
                Id = sectionName,
                Title = dto.Title,
                Description = dto.Description,
                Url = dto.Url,
                BackgroundImage = dto.Image,
                Category = dto.Category,
                Collections = collections
            };
        }).ToList();
        
        return sections;
    }

    /// <summary>
    /// Get a single section by ID
    /// </summary>
    public async Task<Section?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var sections = await GetAllAsync(cancellationToken);
        return sections.FirstOrDefault(s => s.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    }
}
