using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Persistence for RSS feed configurations.
/// Feeds are stored in the database and manageable from the admin UI.
/// </summary>
public interface IRssFeedConfigRepository
{
    /// <summary>Gets all enabled feed configurations.</summary>
    Task<IReadOnlyList<FeedConfig>> GetEnabledAsync(CancellationToken ct = default);

    /// <summary>Gets all feed configurations (including disabled).</summary>
    Task<IReadOnlyList<FeedConfig>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Gets a single feed configuration by ID, or null if not found.</summary>
    Task<FeedConfig?> GetByIdAsync(long id, CancellationToken ct = default);

    /// <summary>Creates a new feed configuration and returns the assigned ID.</summary>
    Task<long> CreateAsync(FeedConfig feed, CancellationToken ct = default);

    /// <summary>Updates an existing feed configuration. Returns true if found and updated.</summary>
    Task<bool> UpdateAsync(FeedConfig feed, CancellationToken ct = default);

    /// <summary>Deletes a feed configuration by ID. Returns true if found and deleted.</summary>
    Task<bool> DeleteAsync(long id, CancellationToken ct = default);
}
