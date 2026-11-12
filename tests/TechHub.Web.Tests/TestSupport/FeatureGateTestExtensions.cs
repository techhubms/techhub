using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using TechHub.Core.Models.FeatureToggles;
using TechHub.Web.Services;

namespace TechHub.Web.Tests.TestSupport;

/// <summary>The parts of the feature gate that a test can change after <see cref="FeatureGateTestExtensions.AddFeatureGate"/>.</summary>
internal sealed class FeatureGateSetup
{
    public FeatureGateSetup(FeatureToggleCache cache, BunitAuthorizationContext authorization)
    {
        Cache = cache;
        Authorization = authorization;
    }

    /// <summary>The toggles that the gate reads.</summary>
    public FeatureToggleCache Cache { get; }

    /// <summary>The visitor: <c>SetAuthorized</c> makes them an admin.</summary>
    public BunitAuthorizationContext Authorization { get; }

    /// <summary>Replaces all toggles.</summary>
    public void SetToggles(params (string Key, FeatureToggleMode Mode)[] toggles) =>
        Cache.Initialize(toggles.Select(t => new FeatureToggle { Key = t.Key, Mode = t.Mode, Description = string.Empty }));
}

/// <summary>
/// Registers the real <see cref="FeatureGate"/> on a bUnit context, with a feature toggle cache that holds the given toggles
/// and a visitor that is an admin or not. Without toggles every feature is disabled.
/// </summary>
internal static class FeatureGateTestExtensions
{
    public static FeatureGateSetup AddFeatureGate(
        this BunitContext context,
        bool isAdmin = false,
        params (string Key, FeatureToggleMode Mode)[] toggles)
    {
        ArgumentNullException.ThrowIfNull(context);

        var authorization = context.AddAuthorization();
        if (isAdmin)
        {
            authorization.SetAuthorized("admin@example.com");
        }
        else
        {
            authorization.SetNotAuthorized();
        }

        var setup = new FeatureGateSetup(new FeatureToggleCache(), authorization);
        setup.SetToggles(toggles);

        context.Services.AddSingleton(setup.Cache);
        context.Services.AddScoped<IFeatureGate, FeatureGate>();
        return setup;
    }
}
