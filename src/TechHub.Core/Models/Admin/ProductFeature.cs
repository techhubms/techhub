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

    /// <summary>Short description shown when the feature card is opened on the features page.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Release date set by an admin. When null the page uses the first launch, preview, ga or release date of the events.</summary>
    public DateOnly? ReleaseDate { get; set; }

    /// <summary>Number of timeline events (dates) that match this feature.</summary>
    public int EventCount { get; set; }

    /// <summary>Content items (videos, posts) the feature links to, in display order.</summary>
    public IReadOnlyList<ProductFeatureContent> Content { get; set; } = [];

    /// <summary>Other names for this feature.</summary>
    public IReadOnlyList<FeatureAlias> Aliases { get; set; } = [];
}

/// <summary>A feature name the AI returned for an event of a product that matches none of the features yet.</summary>
public sealed class UnmatchedFeatureName
{
    /// <summary>The name as the AI returned it.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Number of timeline events with this name.</summary>
    public int EventCount { get; set; }
}

/// <summary>The curated features of a product and the feature names of its dates that are not curated yet.</summary>
public sealed class ProductFeatureOverview
{
    /// <summary>The features of the product, by name.</summary>
    public IReadOnlyList<ProductFeature> Features { get; set; } = [];

    /// <summary>Feature names of the dates of the product (and its child products) that match no feature, most used first.</summary>
    public IReadOnlyList<UnmatchedFeatureName> Unmatched { get; set; } = [];
}
