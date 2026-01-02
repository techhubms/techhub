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
        [JsonPropertyName("Id")]
        public required string Id { get; set; }
        
        [JsonPropertyName("Title")]
        public required string Title { get; set; }
        
        [JsonPropertyName("Description")]
        public required string Description { get; set; }
        
        [JsonPropertyName("Url")]
        public required string Url { get; set; }
        
        [JsonPropertyName("BackgroundImage")]
        public required string BackgroundImage { get; set; }
        
        [JsonPropertyName("Category")]
        public required string Category { get; set; }
        
        [JsonPropertyName("Collections")]
        public required List<CollectionJsonDto> Collections { get; set; }
    }

    /// <summary>
    /// Internal DTO for deserializing collection references from sections.json
    /// </summary>
    private class CollectionJsonDto
    {
        [JsonPropertyName("Title")]
        public required string Title { get; set; }
        
        [JsonPropertyName("Collection")]
        public string? Collection { get; set; } // Nullable for custom pages that don't have a collection
        
        [JsonPropertyName("Url")]
        public required string Url { get; set; }
        
        [JsonPropertyName("Description")]
        public required string Description { get; set; }
        
        [JsonPropertyName("IsCustom")]
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
        
        // sections.json structure: Array of section objects
        var sectionDtos = JsonSerializer.Deserialize<List<SectionJsonDto>>(json, _jsonOptions);
        
        if (sectionDtos == null || sectionDtos.Count == 0)
        {
            return new List<Section>();
        }

        // Convert DTOs to Section models
        var sections = sectionDtos.Select(dto =>
        {
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
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                Url = dto.Url,
                BackgroundImage = dto.BackgroundImage,
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
