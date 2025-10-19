namespace techhub.webapp.ApiService.Domain.Models;

/// <summary>
/// Represents video content from YouTube.
/// </summary>
public class Video : ContentItem
{
    public Video()
    {
        CollectionType = "videos";
    }

    /// <summary>
    /// YouTube video ID (required).
    /// </summary>
    public required string VideoId { get; init; }

    /// <summary>
    /// Video duration in seconds (optional).
    /// </summary>
    public int? Duration { get; set; }

    /// <summary>
    /// Video thumbnail URL (auto-generated from YouTube).
    /// </summary>
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// GitHub Copilot subscription plans that support this feature.
    /// Used for feature videos in ghc-features/ subfolder.
    /// Values: "Free", "Pro", "Business", "Pro+", "Enterprise"
    /// </summary>
    public List<string>? Plans { get; set; }

    /// <summary>
    /// Whether this feature is supported in GitHub Enterprise Server.
    /// Used for feature videos in ghc-features/ subfolder.
    /// </summary>
    public bool? GhesSupport { get; set; }

    /// <summary>
    /// Validates the video item including video-specific rules.
    /// </summary>
    public override void Validate()
    {
        base.Validate();

        if (string.IsNullOrWhiteSpace(VideoId))
            throw new ArgumentException("VideoId cannot be empty", nameof(VideoId));

        if (Duration.HasValue && Duration.Value <= 0)
            throw new ArgumentException("Duration cannot be negative", nameof(Duration));
    }
}
