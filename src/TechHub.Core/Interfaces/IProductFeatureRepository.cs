using TechHub.Core.Models.Admin;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Admin operations on the curated features of a product (<c>product_features</c>). The AI returns the feature of an
/// event as free text; the features link those names to one feature per product, so the events of a feature can be
/// compared across surfaces and plans. Every change updates the links of the events. A feature is always addressed
/// together with its product, so a feature of another product is never touched.
/// </summary>
public interface IProductFeatureRepository
{
    /// <summary>Gets the features of a product with aliases and usage counts, and the feature names of its dates that are not curated yet.</summary>
    Task<ProductFeatureOverview> GetOverviewAsync(long productId, CancellationToken ct = default);

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
}
