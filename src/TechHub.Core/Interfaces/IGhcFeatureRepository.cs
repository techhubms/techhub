using TechHub.Core.Models;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository for the pages of GitHub Copilot: the <c>vscode_update_items</c> table (the content items shown on the VS Code Updates
/// page) and the features of the catalog that the features page shows.
/// </summary>
public interface IGhcFeatureRepository
{
    /// <summary>
    /// Gets the features of a product and of its child products that have content, newest release first (features without a
    /// release date last). A feature has content when an admin linked an item to it, a dated event names it, or an article lists
    /// it by name or alias. Features without content are left out. Returns an empty list when the name is no product.
    /// </summary>
    Task<IReadOnlyList<SectionFeature>> GetFeaturesWithContentAsync(string productName, CancellationToken ct = default);

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
