using TechHub.Core.Models;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Repository for the <c>vscode_update_items</c> table: the content items shown on the VS Code Updates page.
/// </summary>
public interface IGhcFeatureRepository
{
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
