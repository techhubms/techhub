using TechHub.Core.Models.FeatureToggles;

namespace TechHub.Web.Services;

/// <summary>
/// In-memory copy of the feature toggles, so components can ask what is enabled without an API call per render.
/// Refreshed from the API by <see cref="FeatureToggleCacheRefreshService"/> and right after an admin changes a toggle.
/// A feature that is not in the cache is disabled, so a failed first load hides features instead of exposing them.
/// </summary>
public sealed class FeatureToggleCache
{
    private IReadOnlyDictionary<string, FeatureToggleMode> _modes = new Dictionary<string, FeatureToggleMode>();

    /// <summary>Replaces all toggles with <paramref name="toggles"/>.</summary>
    public void Initialize(IEnumerable<FeatureToggle> toggles)
    {
        ArgumentNullException.ThrowIfNull(toggles);

        // A single reference swap: readers see the old or the new set, never a mix
        Volatile.Write(ref _modes, toggles.ToDictionary(t => t.Key, t => t.Mode, StringComparer.Ordinal));
    }

    /// <summary>Who can use the feature. Disabled for a feature the cache does not know.</summary>
    public FeatureToggleMode GetMode(string featureKey) =>
        Volatile.Read(ref _modes).TryGetValue(featureKey, out var mode) ? mode : FeatureToggleMode.Disabled;
}
