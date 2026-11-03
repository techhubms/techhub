using TechHub.Core.Models.Admin;

namespace TechHub.Web.Components;

/// <summary>Everything <see cref="ReviewOverview"/> shows. Persisted between the server render and the interactive render.</summary>
public sealed class ReviewOverviewData
{
    /// <summary>The catalog products, to pick an alias target.</summary>
    public IReadOnlyList<ProductSummary> Products { get; set; } = [];

    /// <summary>Product names the AI returned that are not in the catalog yet.</summary>
    public IReadOnlyList<UnmatchedProductName> UnmatchedProducts { get; set; } = [];

    /// <summary>Names marked as not a product.</summary>
    public IReadOnlyList<string> IgnoredProducts { get; set; } = [];

    /// <summary>Plan and surface names that match nothing yet.</summary>
    public IReadOnlyList<UnmatchedQualifierName> UnmatchedQualifiers { get; set; } = [];

    /// <summary>Products with feature names that match no feature yet.</summary>
    public IReadOnlyList<ProductUnmatchedFeatures> UnmatchedFeatures { get; set; } = [];

    /// <summary>Content fixer proposals waiting for a decision.</summary>
    public int PendingContentFixes { get; set; }

    /// <summary>URLs that failed processing.</summary>
    public int FailedUrls { get; set; }
}
