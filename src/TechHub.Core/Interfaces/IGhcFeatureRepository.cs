using TechHub.Core.Models;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository for the features of GitHub Copilot shown on the features page: the features of the GitHub Copilot product in
/// the catalog (<c>product_features</c> and <c>product_feature_content</c>, edited through <see cref="IProductFeatureRepository"/>), plus the
/// <c>vscode_update_items</c> table.
/// </summary>
public interface IGhcFeatureRepository
{
    // ── GHC Features ──────────────────────────────────────────────────────────

    /// <summary>
    /// Returns the features of GitHub Copilot (including its child products) with their content links, newest release date
    /// first, then newest created.
    /// </summary>
    Task<IReadOnlyList<GhcFeature>> GetAllFeaturesAsync(CancellationToken ct = default);

    /// <summary>Returns a single feature by slug, or null if not found.</summary>
    Task<GhcFeature?> GetFeatureBySlugAsync(string slug, CancellationToken ct = default);

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
