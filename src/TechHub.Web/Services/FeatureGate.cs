using Microsoft.AspNetCore.Components.Authorization;
using TechHub.Core.Models.FeatureToggles;

namespace TechHub.Web.Services;

/// <summary>
/// Decides per visitor whether a feature is available: from the feature toggle and from whether the visitor is signed in.
/// Signing in is only possible for admins, and nothing on the public pages tells a visitor that it exists.
/// </summary>
public interface IFeatureGate
{
    /// <summary>
    /// Whether the visitor is signed in. A component asks this once and then calls <see cref="IsEnabled"/>
    /// on every render, so a changed toggle shows up without asking again.
    /// </summary>
    ValueTask<bool> IsAdminAsync();

    /// <summary>
    /// Whether a visitor can use the feature. <c>null</c> means no feature, which is always available.
    /// A feature that has no toggle is disabled.
    /// </summary>
    bool IsEnabled(string? featureKey, bool isAdmin);

    /// <summary>Whether the current visitor can use the feature.</summary>
    ValueTask<bool> IsEnabledAsync(string? featureKey);
}

/// <inheritdoc />
public sealed class FeatureGate : IFeatureGate
{
    private readonly FeatureToggleCache _cache;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public FeatureGate(FeatureToggleCache cache, AuthenticationStateProvider authenticationStateProvider)
    {
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(authenticationStateProvider);

        _cache = cache;
        _authenticationStateProvider = authenticationStateProvider;
    }

    /// <inheritdoc />
    public async ValueTask<bool> IsAdminAsync()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return state.User.Identity?.IsAuthenticated == true;
    }

    /// <inheritdoc />
    public bool IsEnabled(string? featureKey, bool isAdmin) =>
        featureKey is null || _cache.GetMode(featureKey).IsEnabledFor(isAdmin);

    /// <inheritdoc />
    public async ValueTask<bool> IsEnabledAsync(string? featureKey)
    {
        // Only a feature for admins needs the visitor: the others are the same for everyone
        var isAdmin = featureKey is not null && _cache.GetMode(featureKey) == FeatureToggleMode.Admins && await IsAdminAsync();
        return IsEnabled(featureKey, isAdmin);
    }
}
