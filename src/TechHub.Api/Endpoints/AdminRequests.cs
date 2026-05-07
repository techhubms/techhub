namespace TechHub.Api.Endpoints;

/// <summary>DTO for applying a manually-provided transcript to an existing video content item.</summary>
public sealed class ApplyTranscriptRequest
{
    /// <summary>The full transcript text to use for AI re-processing.</summary>
    public string? Transcript { get; init; }
}

/// <summary>DTO for creating/updating RSS feed configurations.</summary>
public sealed class FeedConfigRequest
{
    public string? Name { get; init; }
    public string? Url { get; init; }
    public string? OutputDir { get; init; }
    public bool Enabled { get; init; } = true;
}

/// <summary>DTO for updating custom page raw JSON.</summary>
public sealed class CustomPageUpdateRequest
{
    public string? JsonData { get; init; }
}

/// <summary>DTO for updating a content item's ai_metadata JSON.</summary>
public sealed class ContentItemMetadataRequest
{
    public string? AiMetadata { get; init; }
}

/// <summary>DTO for updating a background job's enabled state.</summary>
public sealed class JobSettingUpdateRequest
{
    public bool Enabled { get; init; }
}

/// <summary>DTO for rendering markdown to HTML preview.</summary>
public sealed class MarkdownPreviewRequest
{
    public string? Markdown { get; init; }
}

/// <summary>DTO for updating a review's fixed value.</summary>
public sealed class ReviewFixedValueRequest
{
    public string? FixedValue { get; init; }
}

/// <summary>DTO for publishing a draft ghc-features video in-place, replacing it with a real YouTube video.</summary>
public sealed class PublishGhcDraftRequest
{
    /// <summary>The real YouTube URL to assign to this feature (replaces the draft placeholder URL).</summary>
    public string? YoutubeUrl { get; init; }

    /// <summary>
    /// Optional transcript for AI content generation.
    /// When provided, the AI regenerates title, excerpt, content, tags, and sections from the transcript.
    /// When omitted, the AI uses only the existing item metadata.
    /// </summary>
    public string? Transcript { get; init; }

    /// <summary>Subscription tier plan names. At least one is required.</summary>
    public IReadOnlyList<string> Plans { get; init; } = [];

    /// <summary>Whether this feature is available in GitHub Enterprise Server.</summary>
    public bool GhesSupport { get; init; }
}
