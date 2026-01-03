using TechHub.Core.Models;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository for accessing section configuration from sections.json
/// </summary>
public interface ISectionRepository
{
    /// <summary>
    /// Get all sections defined in sections.json
    /// </summary>
    Task<IReadOnlyList<Section>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a single section by ID
    /// </summary>
    Task<Section?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}
