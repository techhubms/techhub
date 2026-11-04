using TechHub.Core.Models.Admin;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Admin operations on the curated features of a product (<c>product_features</c>) and the content they link to. The AI returns the feature of an
/// event as free text; the features link those names to one feature per product, so the events of a feature can be
/// compared. Every change updates the links of the events. A feature is always addressed
/// together with its product, so a feature of another product is never touched.
/// </summary>
public interface IProductFeatureRepository
{
    /// <summary>Gets the features of a product with aliases and usage counts, and the feature names of its dates that are not curated yet.</summary>
    Task<ProductFeatureOverview> GetOverviewAsync(long productId, CancellationToken ct = default);

    /// <summary>Gets the products that have dates with a feature name that matches no feature yet, with the most dates first.</summary>
    Task<IReadOnlyList<ProductUnmatchedFeatures>> GetUnmatchedSummaryAsync(CancellationToken ct = default);

    /// <summary>Adds a feature to a product. Events of the product that use the name are linked to it.</summary>
    Task<ProductOperationResult> CreateAsync(long productId, string name, CancellationToken ct = default);

    /// <summary>Renames a feature. The old name becomes an alias, so events that use it stay linked.</summary>
    Task<ProductOperationResult> RenameAsync(long productId, long id, string name, CancellationToken ct = default);

    /// <summary>Deletes a feature and its aliases. Its events keep their feature text and become unmatched.</summary>
    Task<ProductOperationResult> DeleteAsync(long productId, long id, CancellationToken ct = default);

    /// <summary>Adds an alias to a feature. Events that use the alias are linked to the feature.</summary>
    Task<ProductOperationResult> AddAliasAsync(long productId, long id, string aliasName, CancellationToken ct = default);

    /// <summary>Removes an alias from a feature. Events that used it become unmatched.</summary>
    Task<ProductOperationResult> RemoveAliasAsync(long productId, long id, long aliasId, CancellationToken ct = default);

    /// <summary>Sets the description and the release date of a feature. A null release date makes the page use the dates of the events.</summary>
    Task<ProductOperationResult> UpdateDetailsAsync(long productId, long id, string? description, DateOnly? releaseDate, CancellationToken ct = default);

    /// <summary>
    /// Links a content item to a feature, after the links it already has. With <paramref name="isThumbnail"/> the link becomes the
    /// thumbnail and the previous thumbnail is cleared. Linking an item again only updates its thumbnail flag.
    /// </summary>
    Task<ProductOperationResult> AddContentAsync(long productId, long id, string collectionName, string itemSlug, bool isThumbnail, CancellationToken ct = default);

    /// <summary>Removes the link between a feature and a content item. The content item stays.</summary>
    Task<ProductOperationResult> RemoveContentAsync(long productId, long id, string collectionName, string itemSlug, CancellationToken ct = default);

    /// <summary>Makes a linked content item the thumbnail of the feature, clearing the previous thumbnail.</summary>
    Task<ProductOperationResult> SetThumbnailAsync(long productId, long id, string collectionName, string itemSlug, CancellationToken ct = default);
}
