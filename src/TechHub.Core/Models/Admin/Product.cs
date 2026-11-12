namespace TechHub.Core.Models.Admin;

/// <summary>Another name for a <see cref="ProductSummary"/> (an old name or a spelling variant).</summary>
public sealed class ProductAlias
{
    /// <summary>Database primary key.</summary>
    public long Id { get; set; }

    /// <summary>The alternative name.</summary>
    public string Alias { get; set; } = string.Empty;
}

/// <summary>A product of the catalog by id and name, used for the parent and children of a product.</summary>
public sealed class ProductReference
{
    /// <summary>Database primary key.</summary>
    public long Id { get; set; }

    /// <summary>The canonical product name.</summary>
    public string Name { get; set; } = string.Empty;
}

/// <summary>A product of the catalog with the number of articles and timeline events that are linked to it.</summary>
public sealed class ProductSummary
{
    /// <summary>Database primary key.</summary>
    public long Id { get; set; }

    /// <summary>The canonical product name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Id of the parent product, or null for a top-level product.</summary>
    public long? ParentId { get; set; }

    /// <summary>Name of the parent product, or null for a top-level product.</summary>
    public string? ParentName { get; set; }

    /// <summary>Number of curated features of this product (its own, not those of child products).</summary>
    public int FeatureCount { get; set; }

    /// <summary>Number of products that directly belong to this product.</summary>
    public int ChildCount { get; set; }

    /// <summary>Number of articles that are about this product or one of its child products.</summary>
    public int ItemCount { get; set; }

    /// <summary>Number of timeline events (dates) that are about this product or one of its child products.</summary>
    public int EventCount { get; set; }

    /// <summary>Other names for this product.</summary>
    public IReadOnlyList<ProductAlias> Aliases { get; set; } = [];
}

/// <summary>An article that lists a product.</summary>
public sealed class ProductArticleLink
{
    /// <summary>Collection of the article (primary key part).</summary>
    public string CollectionName { get; set; } = string.Empty;

    /// <summary>Slug of the article (primary key part).</summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>Title of the article.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Publication date of the article as unix epoch seconds.</summary>
    public long DateEpoch { get; set; }

    /// <summary>The name the AI used in this article, which matches a product by name or alias.</summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>The catalog product the name matches: the product itself or one of its children.</summary>
    public string CatalogName { get; set; } = string.Empty;
}

/// <summary>A timeline event (important date) of a product and the article it comes from.</summary>
public sealed class ProductEventLink
{
    /// <summary>Database primary key of the event.</summary>
    public long Id { get; set; }

    /// <summary>Kind of event: preview, ga, retirement, ...</summary>
    public string Kind { get; set; } = string.Empty;

    /// <summary>Feature the event is about, or null for the whole product.</summary>
    public string? Feature { get; set; }

    /// <summary>The curated feature the feature text matches, or null when it matches none.</summary>
    public string? FeatureName { get; set; }

    /// <summary>What happens on the date.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>The date (first day of the month/quarter for coarse precision).</summary>
    public DateOnly EventDate { get; set; }

    /// <summary>Precision of the date: day, month or quarter.</summary>
    public string DatePrecision { get; set; } = "day";

    /// <summary>Collection of the article the event comes from.</summary>
    public string CollectionName { get; set; } = string.Empty;

    /// <summary>Slug of the article the event comes from.</summary>
    public string ItemSlug { get; set; } = string.Empty;

    /// <summary>Title of the article the event comes from.</summary>
    public string ItemTitle { get; set; } = string.Empty;

    /// <summary>The name the AI used for the product in this event.</summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>The catalog product the name matches: the product itself or one of its children.</summary>
    public string CatalogName { get; set; } = string.Empty;
}

/// <summary>
/// A product with its aliases, parent and children and the number of linked articles and dates. The articles and dates
/// themselves are paged (see <c>IProductRepository.GetArticlesAsync</c>), because a parent like GitHub has thousands.
/// </summary>
public sealed class ProductDetail
{
    /// <summary>Database primary key.</summary>
    public long Id { get; set; }

    /// <summary>The canonical product name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>The product this product belongs to, or null for a top-level product.</summary>
    public ProductReference? Parent { get; set; }

    /// <summary>The products that belong to this product, by name.</summary>
    public IReadOnlyList<ProductReference> Children { get; set; } = [];

    /// <summary>Other names for this product.</summary>
    public IReadOnlyList<ProductAlias> Aliases { get; set; } = [];

    /// <summary>Number of article links of this product and its descendants.</summary>
    public int ArticleCount { get; set; }

    /// <summary>Number of timeline events (dates) of this product and its descendants.</summary>
    public int EventCount { get; set; }
}

/// <summary>A product name the AI returned that is not in the catalog (and not ignored) yet.</summary>
public sealed class UnmatchedProductName
{
    /// <summary>The name as the AI returned it.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Number of articles that list this name.</summary>
    public int ItemCount { get; set; }

    /// <summary>Number of timeline events with this name.</summary>
    public int EventCount { get; set; }
}

/// <summary>Outcome of a product catalog change.</summary>
public enum ProductOperationStatus
{
    /// <summary>The change was applied.</summary>
    Success,

    /// <summary>The product, alias or name does not exist.</summary>
    NotFound,

    /// <summary>The change conflicts with the catalog: the name or alias is already used.</summary>
    Conflict,

    /// <summary>The input is not acceptable: empty or too long name, merging a product into itself.</summary>
    Invalid
}

/// <summary>Result of a product catalog change, with a message that can be shown to the admin.</summary>
public sealed record ProductOperationResult(ProductOperationStatus Status, string? Message = null, long? ProductId = null)
{
    /// <summary>True when the change was applied.</summary>
    public bool Succeeded => Status == ProductOperationStatus.Success;

    /// <summary>The change was applied.</summary>
    public static ProductOperationResult Success(long? productId = null) => new(ProductOperationStatus.Success, null, productId);

    /// <summary>The product, alias or name does not exist.</summary>
    public static ProductOperationResult NotFound(string message) => new(ProductOperationStatus.NotFound, message);

    /// <summary>The change conflicts with the catalog.</summary>
    public static ProductOperationResult Conflict(string message) => new(ProductOperationStatus.Conflict, message);

    /// <summary>The input is not acceptable.</summary>
    public static ProductOperationResult Invalid(string message) => new(ProductOperationStatus.Invalid, message);
}
