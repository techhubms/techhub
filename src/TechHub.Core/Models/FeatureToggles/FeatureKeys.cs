namespace TechHub.Core.Models.FeatureToggles;

/// <summary>
/// The keys of the features that have a toggle. Every key has a row in the <c>feature_toggles</c> table;
/// a key without a row is disabled.
/// </summary>
public static class FeatureKeys
{
    /// <summary>The timeline of a section, with its navigation buttons and its page.</summary>
    public const string Timeline = "timeline";
}
