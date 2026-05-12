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

    /// <summary>
    /// Chronological list of GitHub Copilot features for the timeline view.
    /// Each entry includes an explicit release date, description, plan availability, and optional video reference.
    /// </summary>
    public List<TimelineFeature>? TimelineFeatures { get; init; }
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
/// A single feature entry in the GitHub Copilot feature timeline.
/// </summary>
public record TimelineFeature
{
    /// <summary>Unique identifier for the feature (used as HTML anchor and filter key).</summary>
    public required string Id { get; init; }

    /// <summary>Display name of the feature.</summary>
    public required string Name { get; init; }

    /// <summary>
    /// Release date in "YYYY-MM" format (e.g., "2024-05").
    /// Used to sort and group the timeline chronologically.
    /// </summary>
    public required string ReleaseDate { get; init; }

    /// <summary>Brief description of the feature shown on the timeline card.</summary>
    public required string Description { get; init; }

    /// <summary>Subscription tiers this feature is available in (e.g., "Free", "Pro", "Business", "Pro+", "Enterprise").</summary>
    public required List<string> Plans { get; init; }

    /// <summary>Whether this feature is available in GitHub Enterprise Server (GHES).</summary>
    public bool GhesSupport { get; init; }

    /// <summary>
    /// Optional slug of the related ghc-features video content item.
    /// When set, the video thumbnail and link are shown in the expanded feature detail.
    /// </summary>
    public string? VideoSlug { get; init; }
}
