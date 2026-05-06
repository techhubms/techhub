namespace TechHub.Api.Endpoints;

/// <summary>DTO for creating/updating RSS feed configurations.</summary>
public sealed class FeedConfigRequest
{
    public string? Name { get; init; }
    public string? Url { get; init; }
    public string? OutputDir { get; init; }
    public bool Enabled { get; init; } = true;
    public bool TranscriptMandatory { get; init; }
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
