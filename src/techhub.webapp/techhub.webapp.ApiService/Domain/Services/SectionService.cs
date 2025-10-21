namespace techhub.webapp.ApiService.Domain.Services;

using techhub.webapp.ApiService.Domain.Interfaces;
using techhub.webapp.ApiService.Domain.Models;

/// <summary>
/// Implementation of section management logic.
/// Manages sections and collections configuration.
/// </summary>
public class SectionService : ISectionService
{
    private readonly List<Section> _sections = new();
    private readonly object _lock = new();

    public Task<IEnumerable<Section>> GetAllSectionsAsync(CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            return Task.FromResult(_sections.AsEnumerable());
        }
    }

    public Task<Section?> GetSectionByKeyAsync(string sectionKey, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var section = _sections.FirstOrDefault(s => s.Key.Equals(sectionKey, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(section);
        }
    }

    public async Task<IEnumerable<Collection>> GetCollectionsBySectionAsync(string sectionKey, CancellationToken cancellationToken = default)
    {
        var section = await GetSectionByKeyAsync(sectionKey, cancellationToken);
        
        if (section == null)
            return Enumerable.Empty<Collection>();

        return section.Collections;
    }

    public async Task<Collection?> GetCollectionByNameAsync(string sectionKey, string collectionName, CancellationToken cancellationToken = default)
    {
        var section = await GetSectionByKeyAsync(sectionKey, cancellationToken);
        
        if (section == null)
            return null;

        return section.Collections.FirstOrDefault(c => c.Name.Equals(collectionName, StringComparison.OrdinalIgnoreCase));
    }

    public Task<bool> ValidateSectionStructureAsync(CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            try
            {
                foreach (var section in _sections)
                {
                    section.Validate();
                    
                    foreach (var collection in section.Collections)
                    {
                        collection.Validate();
                    }
                }
                
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }

    public Task<IEnumerable<Section>> GetSectionsByCollectionAsync(string collectionName, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var sections = _sections.Where(s =>
                s.Collections.Any(c => c.Name.Equals(collectionName, StringComparison.OrdinalIgnoreCase)));
            
            return Task.FromResult(sections);
        }
    }

    public Task<Section?> GetSectionByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var section = _sections.FirstOrDefault(s => s.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(section);
        }
    }

    /// <summary>
    /// Adds a section to the in-memory collection.
    /// For development/testing purposes.
    /// </summary>
    public void AddSection(Section section)
    {
        if (section == null)
            throw new ArgumentNullException(nameof(section));

        section.Validate();

        lock (_lock)
        {
            if (_sections.Any(s => s.Key.Equals(section.Key, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"Section with key '{section.Key}' already exists");

            _sections.Add(section);
        }
    }

    /// <summary>
    /// Clears all sections from the in-memory collection.
    /// For development/testing purposes.
    /// </summary>
    public void ClearSections()
    {
        lock (_lock)
        {
            _sections.Clear();
        }
    }
}
