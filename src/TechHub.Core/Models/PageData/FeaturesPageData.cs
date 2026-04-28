namespace TechHub.Core.Models;

/// <summary>
/// Data structure for the GitHub Copilot Features page
/// </summary>
public record FeaturesPageData
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Intro { get; init; }
    public required string Note { get; init; }
    public required FeaturesLinks Links { get; init; }
    public required List<SubscriptionTier> SubscriptionTiers { get; init; }
    public required List<FeatureSection> FeatureSections { get; init; }
    public required string VideoCollection { get; init; }
    public List<FeatureTimelineItem> TimelineFeatures { get; init; } = [];
}

public record FeaturesLinks
{
    public required string Pricing { get; init; }
    public required string PlanDetails { get; init; }
}

public record SubscriptionTier
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Tagline { get; init; }
    public TierPrice? Price { get; init; }
    public required List<string> Features { get; init; }
    public required string VideoAnchor { get; init; }
}

public record TierPrice
{
    public required int Monthly { get; init; }
    public int? Yearly { get; init; }
    public bool PerUser { get; init; }
    public required string Currency { get; init; }
}

public record FeatureSection
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required List<string> Plans { get; init; }
}

/// <summary>
/// Represents a single GitHub Copilot feature on the chronological timeline.
/// </summary>
public record FeatureTimelineItem
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }

    /// <summary>ISO 8601 release date (e.g. "2024-06-01").</summary>
    public required string ReleaseDate { get; init; }

    /// <summary>Subscription tiers that include this feature (e.g. ["Free", "Pro"]).</summary>
    public required List<string> Tiers { get; init; }

    public bool GhesSupport { get; init; }

    /// <summary>High-level category (e.g. "Code Completion", "Chat", "Agent").</summary>
    public string? Category { get; init; }

    /// <summary>Slug of the related video in the ghc-features collection, if any.</summary>
    public string? VideoSlug { get; init; }
}
