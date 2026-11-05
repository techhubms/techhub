using TechHub.Core.Models.FeatureToggles;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Persistence for the feature toggles.
/// </summary>
public interface IFeatureToggleRepository
{
    /// <summary>Gets all feature toggles, ordered by key.</summary>
    Task<IReadOnlyList<FeatureToggle>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Sets who can use a feature. Returns false when there is no feature with this key.</summary>
    Task<bool> SetModeAsync(string featureKey, FeatureToggleMode mode, CancellationToken ct = default);
}
