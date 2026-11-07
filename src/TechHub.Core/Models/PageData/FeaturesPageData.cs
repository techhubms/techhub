namespace TechHub.Core.Models;

/// <summary>
/// Data structure of the GitHub Copilot subscriptions: the tiers shown in the sidebar of the GitHub Copilot timeline, with the
/// intro and pricing links that go with them.
/// </summary>
public record FeaturesPageData
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Intro { get; init; }
    public required FeaturesLinks Links { get; init; }
    public BillingNotice? BillingNotice { get; init; }
    public required List<SubscriptionTier> SubscriptionTiers { get; init; }
}

public record FeaturesLinks
{
    public required string Pricing { get; init; }
    public required string PlanDetails { get; init; }
}

public record BillingNotice
{
    public required string Text { get; init; }
    public required IReadOnlyList<BillingNoticeLink> Links { get; init; }
}

public record BillingNoticeLink
{
    public required string Label { get; init; }
    public required string Url { get; init; }
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
