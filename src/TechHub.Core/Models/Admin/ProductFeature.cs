namespace TechHub.Core.Models.Admin;

/// <summary>Another name for a <see cref="ProductFeature"/>.</summary>
public sealed class FeatureAlias
{
    /// <summary>Database primary key.</summary>
    public long Id { get; set; }

    /// <summary>The alternative name.</summary>
    public string Alias { get; set; } = string.Empty;
}

/// <summary>A content item (video, post) that a <see cref="ProductFeature"/> links to.</summary>
public sealed class ProductFeatureContent
{
    /// <summary>Collection of the content item.</summary>
    public string CollectionName { get; set; } = string.Empty;

    /// <summary>Slug of the content item.</summary>
    public string ItemSlug { get; set; } = string.Empty;

    /// <summary>Title of the content item.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Whether this link is the thumbnail of the feature (at most one link per feature is).</summary>
    public bool IsThumbnail { get; set; }

    /// <summary>Whether the article was linked by detected feature mentions or timeline events.</summary>
    public bool IsDetected { get; set; }

    /// <summary>Position among the links of the feature, lowest first.</summary>
    public int SortOrder { get; set; }
}

/// <summary>A curated feature of a product (Agent mode of GitHub Copilot) with its aliases, page data and the number of linked dates.</summary>
public sealed class ProductFeature
{
    /// <summary>Database primary key.</summary>
    public long Id { get; set; }

    /// <summary>The product the feature belongs to.</summary>
    public long ProductId { get; set; }

    /// <summary>The canonical feature name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Short description of the feature.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Release date set by an admin.</summary>
    public DateOnly? ReleaseDate { get; set; }

    /// <summary>Number of timeline events (dates) that match this feature.</summary>
    public int EventCount { get; set; }

    /// <summary>Content items (videos, posts) the feature links to, in display order.</summary>
    public IReadOnlyList<ProductFeatureContent> Content { get; set; } = [];

    /// <summary>Other names for this feature.</summary>
    public IReadOnlyList<FeatureAlias> Aliases { get; set; } = [];
}

/// <summary>A feature name returned by article or event extraction that matches none of the product's features yet.</summary>
public sealed class UnmatchedFeatureName
{
    /// <summary>The name as the AI returned it.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Number of distinct content items mentioning the feature without a date.</summary>
    public int ContentCount { get; set; }

    /// <summary>Number of timeline events with this name.</summary>
    public int EventCount { get; set; }
}

/// <summary>A product with extracted feature names that match none of its features yet.</summary>
public sealed class ProductUnmatchedFeatures
{
    /// <summary>The product the extracted feature names are linked to.</summary>
    public long ProductId { get; set; }

    /// <summary>The name of the product.</summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>Number of different feature names (ignoring case and extra whitespace) to review.</summary>
    public int NameCount { get; set; }

    /// <summary>Number of distinct content items containing unmatched feature names.</summary>
    public int ContentCount { get; set; }

    /// <summary>Number of timeline events with such a name.</summary>
    public int EventCount { get; set; }
}

/// <summary>The curated features of a product and extracted feature names that are not curated yet.</summary>
public sealed class ProductFeatureOverview
{
    /// <summary>The features of the product, by name.</summary>
    public IReadOnlyList<ProductFeature> Features { get; set; } = [];

    /// <summary>Feature names from articles or dates of the product (and its child products) that match no feature, most used first.</summary>
    public IReadOnlyList<UnmatchedFeatureName> Unmatched { get; set; } = [];
}
