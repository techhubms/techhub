using TechHub.Core.Models;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository for the <c>ghc_features</c>, <c>ghc_feature_content</c>, and
/// <c>vscode_update_items</c> tables.
/// </summary>
public interface IGhcFeatureRepository
{
    // ── GHC Features ──────────────────────────────────────────────────────────

    /// <summary>Returns all features ordered by release_date DESC, created_at DESC, slug.</summary>
    Task<IReadOnlyList<GhcFeature>> GetAllFeaturesAsync(CancellationToken ct = default);

    /// <summary>Returns a single feature by slug, or null if not found.</summary>
    Task<GhcFeature?> GetFeatureBySlugAsync(string slug, CancellationToken ct = default);

    /// <summary>
    /// Inserts or updates a feature. Returns true if a row was affected.
    /// Plans CSV is built from <see cref="GhcFeature.Plans"/>.
    /// </summary>
    Task<bool> UpsertFeatureAsync(GhcFeature feature, CancellationToken ct = default);

    /// <summary>Deletes a feature and its content links. Returns true if a row was deleted.</summary>
    Task<bool> DeleteFeatureAsync(string slug, CancellationToken ct = default);

    // ── Content links ─────────────────────────────────────────────────────────

    /// <summary>
    /// Adds a content link. If <paramref name="isThumbnail"/> is true, clears any existing
    /// thumbnail link first (enforcing the one-thumbnail-per-feature constraint).
    /// Returns true if the link was inserted.
    /// </summary>
    Task<bool> AddContentLinkAsync(
        string featureSlug,
        string collectionName,
        string itemSlug,
        bool isThumbnail,
        int sortOrder,
        CancellationToken ct = default);

    /// <summary>Removes a content link. Returns true if a row was deleted.</summary>
    Task<bool> RemoveContentLinkAsync(
        string featureSlug,
        string collectionName,
        string itemSlug,
        CancellationToken ct = default);

    /// <summary>
    /// Marks the specified link as the thumbnail for the feature, clearing any previous thumbnail.
    /// Returns true if the update succeeded.
    /// </summary>
    Task<bool> SetThumbnailAsync(
        string featureSlug,
        string collectionName,
        string itemSlug,
        CancellationToken ct = default);

    // ── VS Code Updates ────────────────────────────────────────────────────────

    /// <summary>
    /// Ensures the given content item is registered as a VS Code Update.
    /// Idempotent — safe to call even if the row already exists.
    /// </summary>
    Task AddVscodeUpdateItemAsync(string collectionName, string slug, CancellationToken ct = default);

    /// <summary>
    /// Returns a paged list of VS Code Update items joined with content_items for display.
    /// </summary>
    Task<(IReadOnlyList<VscodeUpdateListItem> Items, int TotalCount)> GetVscodeUpdateItemsAsync(
        int offset,
        int pageSize,
        string? search,
        CancellationToken ct = default);

    /// <summary>
    /// Removes a content item from the VS Code Updates list (does NOT delete the content item).
    /// Returns true if a row was deleted.
    /// </summary>
    Task<bool> RemoveVscodeUpdateItemAsync(string collectionName, string slug, CancellationToken ct = default);
}
