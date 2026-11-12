using TechHub.Core.Models;
using TechHub.Core.Models.Admin;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Admin operations on the product catalog (<c>products</c>, <c>product_aliases</c>, <c>ignored_product_names</c>).
/// The AI returns product names as free text; the catalog links those names to curated products. Every change that
/// affects which name matches which product updates the links of the articles and timeline events as well.
/// </summary>
public interface IProductRepository
{
    /// <summary>Gets the products with their aliases and usage counts, by name. <paramref name="search"/> filters on the name or an alias.</summary>
    Task<IReadOnlyList<ProductSummary>> GetProductsAsync(string? search = null, CancellationToken ct = default);

    /// <summary>Gets a product with its aliases, parent, children and the number of linked articles and events, or null if it does not exist.</summary>
    Task<ProductDetail?> GetDetailAsync(long id, CancellationToken ct = default);

    /// <summary>
    /// Gets a page of the articles of a product, newest first. With <paramref name="includeChildren"/> the articles of its descendants
    /// are included. <paramref name="pageSize"/> is capped at 100.
    /// </summary>
    Task<PagedResult<ProductArticleLink>> GetArticlesAsync(long id, bool includeChildren, int page, int pageSize, CancellationToken ct = default);

    /// <summary>Gets a page of the timeline events of a product, newest first. With <paramref name="includeChildren"/> the events of its descendants are included.</summary>
    Task<PagedResult<ProductEventLink>> GetEventsAsync(long id, bool includeChildren, int page, int pageSize, CancellationToken ct = default);

    /// <summary>Gets the names the AI returned that are not in the catalog and not ignored, most used first.</summary>
    Task<IReadOnlyList<UnmatchedProductName>> GetUnmatchedNamesAsync(CancellationToken ct = default);

    /// <summary>Gets the names that are marked as not being a product.</summary>
    Task<IReadOnlyList<string>> GetIgnoredNamesAsync(CancellationToken ct = default);

    /// <summary>Adds a product. Articles and events that use the name are linked to it.</summary>
    Task<ProductOperationResult> CreateAsync(string name, CancellationToken ct = default);

    /// <summary>Renames a product. The old name becomes an alias, so articles that use it stay linked.</summary>
    Task<ProductOperationResult> RenameAsync(long id, string name, CancellationToken ct = default);

    /// <summary>Adds an alias to a product. Articles and events that use the alias are linked to the product.</summary>
    Task<ProductOperationResult> AddAliasAsync(long id, string aliasName, CancellationToken ct = default);

    /// <summary>
    /// Makes a product a child of another product (GitHub Copilot CLI belongs to GitHub Copilot), or top-level when
    /// <paramref name="parentId"/> is null. A product cannot be its own ancestor.
    /// </summary>
    Task<ProductOperationResult> SetParentAsync(long id, long? parentId, CancellationToken ct = default);

    /// <summary>Removes an alias from a product. Articles and events that used it become unmatched.</summary>
    Task<ProductOperationResult> RemoveAliasAsync(long id, long aliasId, CancellationToken ct = default);

    /// <summary>
    /// Merges <paramref name="sourceId"/> into <paramref name="targetId"/>: the name and aliases of the source become
    /// aliases of the target, its child products and features move to the target (those the target has
    /// by name already are dropped) and the source product is removed.
    /// </summary>
    Task<ProductOperationResult> MergeAsync(long sourceId, long targetId, CancellationToken ct = default);

    /// <summary>
    /// Turns a child product into a feature of its parent (GitHub Copilot coding agent becomes the feature Coding agent of GitHub Copilot).
    /// Its name and aliases become aliases of the feature, not of the product, so products and features stay separate; its own
    /// child products, and features move to the parent. The result carries the id of the parent.
    /// </summary>
    Task<ProductOperationResult> ConvertToFeatureAsync(long id, CancellationToken ct = default);

    /// <summary>Deletes a product and its aliases. Articles and events that were linked to it become unmatched.</summary>
    Task<ProductOperationResult> DeleteAsync(long id, CancellationToken ct = default);

    /// <summary>Marks a name as not being a product, so it leaves the review queue.</summary>
    Task<ProductOperationResult> IgnoreNameAsync(string name, CancellationToken ct = default);

    /// <summary>Puts an ignored name back in the review queue (when it is used).</summary>
    Task<ProductOperationResult> UnignoreNameAsync(string name, CancellationToken ct = default);
}
