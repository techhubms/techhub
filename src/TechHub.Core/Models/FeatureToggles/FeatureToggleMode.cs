using System.Text.Json.Serialization;

namespace TechHub.Core.Models.FeatureToggles;

/// <summary>Who can use a feature.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FeatureToggleMode
{
    /// <summary>Nobody can use the feature.</summary>
    Disabled = 0,

    /// <summary>Only signed-in admins can use the feature.</summary>
    Admins = 1,

    /// <summary>Everyone can use the feature.</summary>
    Everyone = 2
}

/// <summary>Evaluation of a <see cref="FeatureToggleMode"/>.</summary>
public static class FeatureToggleModeExtensions
{
    /// <summary>Whether a visitor can use a feature in this mode.</summary>
    public static bool IsEnabledFor(this FeatureToggleMode mode, bool isAdmin) => mode switch
    {
        FeatureToggleMode.Everyone => true,
        FeatureToggleMode.Admins => isAdmin,
        _ => false
    };
}
