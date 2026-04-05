namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// Result of a roundup generation attempt.
/// </summary>
public enum RoundupGenerationResult
{
    /// <summary>A new roundup was generated and written to the database.</summary>
    Generated,

    /// <summary>A roundup for this week already exists in the database.</summary>
    AlreadyExists,

    /// <summary>No articles were found for the target week.</summary>
    NoArticles,

    /// <summary>Articles existed but none remained after relevance filtering.</summary>
    NoArticlesAfterFiltering
}

/// <summary>
/// Combines the generation result with the slug of the generated roundup (when applicable).
/// </summary>
public sealed class RoundupGenerationOutcome
{
    public RoundupGenerationResult Result { get; }

    /// <summary>The slug of the generated roundup, or <c>null</c> when no roundup was created.</summary>
    public string? Slug { get; }

    private RoundupGenerationOutcome(RoundupGenerationResult result, string? slug = null)
    {
        Result = result;
        Slug = slug;
    }

    public static RoundupGenerationOutcome Generated(string slug) => new(RoundupGenerationResult.Generated, slug);
    public static RoundupGenerationOutcome AlreadyExists => new(RoundupGenerationResult.AlreadyExists);
    public static RoundupGenerationOutcome NoArticles => new(RoundupGenerationResult.NoArticles);
    public static RoundupGenerationOutcome NoArticlesAfterFiltering => new(RoundupGenerationResult.NoArticlesAfterFiltering);
}
