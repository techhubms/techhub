namespace TechHub.Core.Configuration;

/// <summary>
/// Identifies the AI pipeline step a request belongs to. Each step has its own <see cref="AiProfileOptions"/>.
/// </summary>
public enum AiProfile
{
    /// <summary>Classifies an article (include/exclude, sections, tags, roundup metadata).</summary>
    Classification,

    /// <summary>Rewrites an included article into cleaned-up markdown.</summary>
    Rewrite,

    /// <summary>Writes the body of a weekly section roundup.</summary>
    RoundupNewsWriter,

    /// <summary>Generates roundup title, tags and introduction.</summary>
    RoundupMetadata
}

/// <summary>
/// Model settings for a single AI pipeline step.
/// </summary>
public sealed class AiProfileOptions
{
    /// <summary>Service tier that gives the 50% discount on batch-style workloads.</summary>
    public const string FlexTier = "flex";

    /// <summary>Foundry deployment name, sent as the <c>model</c> of the v1 chat completions API.</summary>
    public string DeploymentName { get; init; } = string.Empty;

    /// <summary>Reasoning effort (<c>none</c>, <c>minimal</c>, <c>low</c>, <c>medium</c>, <c>high</c>), or empty to omit it.</summary>
    public string ReasoningEffort { get; init; } = string.Empty;

    /// <summary>Service tier: <c>default</c> (Standard) or <c>flex</c>.</summary>
    public string ServiceTier { get; init; } = "default";

    /// <summary>Gets a value indicating whether this profile runs on the Flex tier.</summary>
    public bool IsFlex => string.Equals(ServiceTier, FlexTier, StringComparison.OrdinalIgnoreCase);
}

/// <summary>
/// The model settings for each AI pipeline step.
/// </summary>
public sealed class AiProfilesOptions
{
    /// <summary>Article classification.</summary>
    public AiProfileOptions Classification { get; init; } = new();

    /// <summary>Article rewrite.</summary>
    public AiProfileOptions Rewrite { get; init; } = new();

    /// <summary>Roundup writer.</summary>
    public AiProfileOptions RoundupNewsWriter { get; init; } = new();

    /// <summary>Roundup metadata.</summary>
    public AiProfileOptions RoundupMetadata { get; init; } = new();

    /// <summary>Returns the settings for <paramref name="profile"/>.</summary>
    public AiProfileOptions Get(AiProfile profile) => profile switch
    {
        AiProfile.Classification => Classification,
        AiProfile.Rewrite => Rewrite,
        AiProfile.RoundupNewsWriter => RoundupNewsWriter,
        AiProfile.RoundupMetadata => RoundupMetadata,
        _ => throw new ArgumentOutOfRangeException(nameof(profile), profile, "Unknown AI profile")
    };
}
