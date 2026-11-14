namespace TechHub.Core.Models;

/// <summary>
/// A feature from the product catalog that has content (a video or an article) to show on the features page of a section.
/// </summary>
public sealed record SectionFeature
{
    /// <summary>Readable address of the feature, unique across the catalog.</summary>
    public required string Slug { get; init; }

    /// <summary>The name of the feature (Agent mode).</summary>
    public required string Name { get; init; }

    /// <summary>The product the feature belongs to; a child product of the section's product shows its own name.</summary>
    public required string ProductName { get; init; }

    /// <summary>Short description of the feature, empty when none is written.</summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>The day the feature was released, when an admin set it.</summary>
    public DateOnly? ReleaseDate { get; init; }

    /// <summary>The content that belongs to the feature: the thumbnail first, then the curated links, then the articles the catalog detected.</summary>
    public required IReadOnlyList<SectionFeatureContent> Content { get; init; }
}

/// <summary>A content item that belongs to a <see cref="SectionFeature"/>.</summary>
public sealed record SectionFeatureContent
{
    public required string CollectionName { get; init; }

    public required string ItemSlug { get; init; }

    public required string Title { get; init; }

    /// <summary>The URL of the original (the YouTube address of a video); empty for content without one.</summary>
    public string ExternalUrl { get; init; } = string.Empty;

    /// <summary>The section the item belongs to, used for its address.</summary>
    public required string PrimarySectionName { get; init; }

    /// <summary>Whether an admin chose this item as the thumbnail of the feature.</summary>
    public bool IsThumbnail { get; init; }

    /// <summary>Whether the link comes from the catalog (detected in articles and dates) instead of from an admin.</summary>
    public bool IsDetected { get; init; }

    /// <summary>The address of the item: its own page, or the original for news, blogs and community items.</summary>
    public string GetHref() => ContentItem.BuildHref(CollectionName, ItemSlug, ExternalUrl, PrimarySectionName);

    /// <summary>The YouTube video id of <see cref="ExternalUrl"/> (youtu.be and youtube.com addresses), or null when the item is no YouTube video.</summary>
    public string? GetYouTubeVideoId()
    {
        if (!Uri.TryCreate(ExternalUrl, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp))
        {
            return null;
        }

        string? id = null;
        if (uri.Host.Equals("youtu.be", StringComparison.OrdinalIgnoreCase))
        {
            id = uri.AbsolutePath.Trim('/');
        }
        else if (uri.Host.Equals("youtube.com", StringComparison.OrdinalIgnoreCase)
                 || uri.Host.EndsWith(".youtube.com", StringComparison.OrdinalIgnoreCase))
        {
            id = uri.Query.TrimStart('?')
                .Split('&', StringSplitOptions.RemoveEmptyEntries)
                .Select(pair => pair.Split('=', 2))
                .Where(parts => parts.Length == 2 && parts[0] == "v")
                .Select(parts => parts[1])
                .FirstOrDefault();
        }

        // The id ends up in an image address: only the characters of a real id are accepted.
        return id is { Length: > 0 and <= 20 } && id.All(c => char.IsAsciiLetterOrDigit(c) || c is '-' or '_') ? id : null;
    }
}
