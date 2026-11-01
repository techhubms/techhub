namespace TechHub.Core.Models.Admin;

/// <summary>Another name for a <see cref="ProductFeature"/>.</summary>
public sealed class FeatureAlias
{
    /// <summary>Database primary key.</summary>
    public long Id { get; set; }

    /// <summary>The alternative name.</summary>
    public string Alias { get; set; } = string.Empty;
}

/// <summary>A curated feature of a product (Agent mode of GitHub Copilot) with its aliases and the number of linked dates.</summary>
public sealed class ProductFeature
{
    /// <summary>Database primary key.</summary>
    public long Id { get; set; }

    /// <summary>The product the feature belongs to.</summary>
    public long ProductId { get; set; }

    /// <summary>The canonical feature name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Number of timeline events (dates) that match this feature.</summary>
    public int EventCount { get; set; }

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
