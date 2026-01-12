using TechHub.Core.Models;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository for managing custom pages from the _custom collection
/// Custom pages are standalone content pages that don't belong to standard collections
/// </summary>
public interface ICustomPageRepository
{
    /// <summary>
    /// Get all custom pages
    /// </summary>
    Task<IReadOnlyList<CustomPage>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a custom page by its slug
    /// </summary>
    Task<CustomPage?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
