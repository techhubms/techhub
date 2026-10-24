namespace TechHub.Api.Endpoints;

/// <summary>DTO for applying a manually-provided transcript to an existing video content item.</summary>
public sealed class ApplyTranscriptRequest
{
    /// <summary>The full transcript text to use for AI re-processing.</summary>
    public string? Transcript { get; init; }
}

/// <summary>DTO for starting the timeline backfill job.</summary>
public sealed class TimelineBackfillRequestDto
{
    /// <summary>Scan articles that were already scanned again.</summary>
    public bool Force { get; init; }

    /// <summary>Only articles published on or after this date (yyyy-MM-dd).</summary>
    public string? PublishedAfter { get; init; }

    /// <summary>Only articles of this collection (e.g. "news").</summary>
    public string? Collection { get; init; }

    /// <summary>Maximum number of articles to scan.</summary>
    public int? Limit { get; init; }
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

public sealed class NewsletterSubscribeRequest
{
    public string? Email { get; init; }
    public string? DisplayName { get; init; }
    public IReadOnlyList<string>? WeeklySections { get; init; }
    public IReadOnlyList<string>? DailySections { get; init; }
}

public sealed class NewsletterUnsubscribeRequest
{
    public string? Email { get; init; }
    public string? Token { get; init; }
}

public sealed class NewsletterSubscriberUpdateRequest
{
    public string? DisplayName { get; init; }
    public IReadOnlyList<string>? WeeklySections { get; init; }
    public IReadOnlyList<string>? DailySections { get; init; }
}

public sealed class NewsletterManageUpdateRequest
{
    public string? Email { get; init; }
    public string? Token { get; init; }
    public string? DisplayName { get; init; }
    public IReadOnlyList<string>? WeeklySections { get; init; }
    public IReadOnlyList<string>? DailySections { get; init; }
}
