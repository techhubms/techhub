namespace TechHub.Core.Models.Admin;

/// <summary>
/// Request body for updating subscription plans, GHES support, and draft status
/// of a ghc-features content item.
/// </summary>
public sealed record GhcFeaturePlansUpdateRequest
{
    /// <summary>
    /// Subscription tier names this feature belongs to.
    /// Must contain at least one valid plan: Free, Student, Pro, Business, Pro+, Enterprise.
    /// </summary>
    public required IReadOnlyList<string> Plans { get; init; }

    /// <summary>
    /// Whether this feature is available in GitHub Enterprise Server.
    /// </summary>
    public bool GhesSupport { get; init; }

    /// <summary>
    /// When <c>true</c> the feature is a draft / coming soon and is not yet publicly visible.
    /// </summary>
    public bool Draft { get; init; }
}
