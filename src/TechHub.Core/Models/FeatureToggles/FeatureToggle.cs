namespace TechHub.Core.Models.FeatureToggles;

/// <summary>A feature that can be switched off, or limited to admins, without a deployment.</summary>
public class FeatureToggle
{
    /// <summary>Unique key of the feature, one of the constants of <see cref="FeatureKeys"/>.</summary>
    public required string Key { get; init; }

    /// <summary>Who can use the feature.</summary>
    public FeatureToggleMode Mode { get; init; }

    /// <summary>What the feature is, for the admin page.</summary>
    public required string Description { get; init; }
}
