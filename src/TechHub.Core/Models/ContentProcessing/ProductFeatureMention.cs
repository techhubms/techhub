namespace TechHub.Core.Models.ContentProcessing;

/// <summary>A product feature the article is about, whether or not the article states a date for it.</summary>
public sealed record ProductFeatureMention
{
    /// <summary>The product the feature belongs to.</summary>
    public string Product { get; init; } = string.Empty;

    /// <summary>The feature of the product.</summary>
    public string Feature { get; init; } = string.Empty;
}
