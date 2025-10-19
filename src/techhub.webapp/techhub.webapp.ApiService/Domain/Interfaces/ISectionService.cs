using techhub.webapp.ApiService.Domain.Models;

namespace techhub.webapp.ApiService.Domain.Interfaces;

/// <summary>
/// Section service interface for managing sections and collections
/// </summary>
public interface ISectionService
{
    /// <summary>
    /// Retrieves all sections
    /// </summary>
    Task<IEnumerable<Section>> GetAllSectionsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific section by key
    /// </summary>
    /// <param name="key">Section key (e.g., "ai", "github-copilot")</param>
    Task<Section?> GetSectionByKeyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all collections for a specific section
    /// </summary>
    /// <param name="sectionKey">Section key</param>
    Task<IEnumerable<Collection>> GetCollectionsBySectionAsync(string sectionKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific collection by name within a section
    /// </summary>
    /// <param name="sectionKey">Section key</param>
    /// <param name="collectionName">Collection name (news, posts, videos, community, events, roundups)</param>
    Task<Collection?> GetCollectionByNameAsync(string sectionKey, string collectionName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates section structure and configuration
    /// </summary>
    /// <returns>True if valid, throws exception with details if invalid</returns>
    Task<bool> ValidateSectionStructureAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves sections containing specific collection type
    /// </summary>
    /// <param name="collectionName">Collection name to find</param>
    Task<IEnumerable<Section>> GetSectionsByCollectionAsync(string collectionName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets section by category
    /// </summary>
    /// <param name="category">Category name (AI, GitHub Copilot, ML, Azure, Coding, DevOps, Security, All)</param>
    Task<Section?> GetSectionByCategoryAsync(string category, CancellationToken cancellationToken = default);
}
