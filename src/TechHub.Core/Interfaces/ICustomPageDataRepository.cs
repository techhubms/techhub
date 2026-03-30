using TechHub.Core.Models.Admin;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Persistence for custom page JSON data.
/// Custom pages are stored in the database and manageable from the admin UI.
/// </summary>
public interface ICustomPageDataRepository
{
    /// <summary>Gets all custom page entries (key, description, updatedAt — without full JSON).</summary>
    Task<IReadOnlyList<CustomPageEntry>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Gets a single custom page entry by key, or null if not found.</summary>
    Task<CustomPageEntry?> GetByKeyAsync(string key, CancellationToken ct = default);

    /// <summary>Creates or updates the JSON data for a custom page key.</summary>
    Task UpsertAsync(string key, string description, string jsonData, CancellationToken ct = default);

    /// <summary>Returns true if the table has no rows (used for seeding on first run).</summary>
    Task<bool> IsEmptyAsync(CancellationToken ct = default);
}
