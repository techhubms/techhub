using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechHub.Core.Configuration;
using TechHub.Core.Interfaces;

namespace TechHub.Infrastructure.Services;

/// <summary>
/// Fetches YouTube video tags from the YouTube snippet API.
/// Mirrors the PowerShell <c>GetYouTubeTags</c> logic in <c>Feed.ps1</c>.
/// </summary>
public class YouTubeTagService : IYouTubeTagService
{
    private readonly IYouTubeTagClient _tagClient;
    private readonly ContentProcessorOptions _options;
    private readonly ILogger<YouTubeTagService> _logger;

    public YouTubeTagService(
        IYouTubeTagClient tagClient,
        IOptions<ContentProcessorOptions> options,
        ILogger<YouTubeTagService> logger)
    {
        ArgumentNullException.ThrowIfNull(tagClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        _tagClient = tagClient;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Fetches tags for a YouTube video URL. Returns an empty list when tags
    /// cannot be retrieved, the video has no tags, or the tag count exceeds
    /// <see cref="ContentProcessorOptions.MaxYouTubeTagCount"/>.
    /// </summary>
    public virtual async Task<IReadOnlyList<string>> GetTagsAsync(string videoUrl, CancellationToken ct = default)
    {
        var videoId = ExtractVideoId(videoUrl);
        if (string.IsNullOrEmpty(videoId))
        {
            return [];
        }

        try
        {
            var json = await _tagClient.FetchVideoSnippetAsync(videoId, ct);
            if (json is null)
            {
                return [];
            }

            var tags = ParseTags(json);

            if (tags.Count == 0)
            {
                _logger.LogDebug("No tags found for YouTube video {VideoId}", videoId);
                return [];
            }

            // Skip channels that stuff every video with the same huge list of SEO tags
            if (tags.Count > _options.MaxYouTubeTagCount)
            {
                _logger.LogDebug(
                    "Skipping {Count} tags for YouTube video {VideoId} (exceeds max {Max})",
                    tags.Count, videoId, _options.MaxYouTubeTagCount);
                return [];
            }

            _logger.LogDebug("Found {Count} tags for YouTube video {VideoId}", tags.Count, videoId);
            return tags;
        }
        catch (Exception ex) when (ex is JsonException or IOException)
        {
            _logger.LogWarning(ex, "Failed to parse YouTube tags for video {VideoId}", videoId);
            return [];
        }
    }

    /// <summary>
    /// Extracts the YouTube video ID from various URL formats (watch, embed, short, youtu.be).
    /// </summary>
    internal static string? ExtractVideoId(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            return null;
        }

        var host = uri.Host.ToLowerInvariant();

        // youtu.be/VIDEO_ID
        if (host is "youtu.be")
        {
            var path = uri.AbsolutePath.TrimStart('/');
            return IsValidVideoId(path) ? path : null;
        }

        // youtube.com variants
        if (host is "youtube.com" or "www.youtube.com" or "m.youtube.com")
        {
            // /watch?v=VIDEO_ID
            if (uri.AbsolutePath.Equals("/watch", StringComparison.OrdinalIgnoreCase))
            {
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                var v = query["v"];
                return IsValidVideoId(v) ? v : null;
            }

            // /embed/VIDEO_ID, /v/VIDEO_ID, /shorts/VIDEO_ID
            var match = Regex.Match(uri.AbsolutePath, @"^/(embed|v|shorts)/([a-zA-Z0-9_-]{11})", RegexOptions.None, TimeSpan.FromSeconds(1));
            if (match.Success)
            {
                return match.Groups[2].Value;
            }
        }

        return null;
    }

    private static bool IsValidVideoId(string? id) =>
        !string.IsNullOrEmpty(id) && Regex.IsMatch(id, @"^[a-zA-Z0-9_-]{11}$", RegexOptions.None, TimeSpan.FromSeconds(1));

    private static List<string> ParseTags(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("items", out var items) || items.GetArrayLength() == 0)
            {
                return [];
            }

            var snippet = items[0];
            if (!snippet.TryGetProperty("snippet", out var snippetObj))
            {
                return [];
            }

            if (!snippetObj.TryGetProperty("tags", out var tagsArray))
            {
                return [];
            }

            var tags = tagsArray.EnumerateArray()
                .Select(tag => tag.GetString())
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => value!.Trim())
                .ToList();

            return tags;
        }
        catch (JsonException)
        {
            return [];
        }
    }
}
