using TechHub.Core.Models.Admin;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Admin operations on the vocabulary of plans and surfaces (<c>product_qualifiers</c>). The AI returns the plans and
/// surfaces of an event as free text; the vocabulary links those names to curated qualifiers, and a product alias can
/// imply a qualifier. Every change updates the links of the articles and events.
/// </summary>
public interface IProductQualifierRepository
{
    /// <summary>Gets the plans and surfaces with their aliases and usage counts, by kind and name.</summary>
    Task<IReadOnlyList<ProductQualifier>> GetQualifiersAsync(CancellationToken ct = default);

    /// <summary>Gets the plan and surface names the AI returned that are not in the vocabulary, most used first.</summary>
    Task<IReadOnlyList<UnmatchedQualifierName>> GetUnmatchedNamesAsync(CancellationToken ct = default);

    /// <summary>Adds a plan or surface. Events that use the name are linked to it.</summary>
    Task<ProductOperationResult> CreateAsync(string kind, string name, CancellationToken ct = default);

    /// <summary>Renames a qualifier. The old name becomes an alias, so events that use it stay linked.</summary>
    Task<ProductOperationResult> RenameAsync(long id, string name, CancellationToken ct = default);

    /// <summary>Deletes a qualifier and its aliases. Events and product aliases that used it lose it.</summary>
    Task<ProductOperationResult> DeleteAsync(long id, CancellationToken ct = default);

    /// <summary>Adds an alias to a qualifier. Events that use the alias are linked to the qualifier.</summary>
    Task<ProductOperationResult> AddAliasAsync(long id, string aliasName, CancellationToken ct = default);

    /// <summary>Removes an alias from a qualifier. Events that used it lose the qualifier.</summary>
    Task<ProductOperationResult> RemoveAliasAsync(long id, long aliasId, CancellationToken ct = default);
}
