namespace TechHub.Core.Interfaces;

/// <summary>
/// Fetches YouTube video tags from the YouTube snippet API.
/// </summary>
public interface IYouTubeTagService
{
    /// <summary>
    /// Fetches tags for a YouTube video URL. Returns an empty list when tags
    /// cannot be retrieved, the video has no tags, or the tag count exceeds
    /// the configured maximum.
    /// </summary>
    Task<IReadOnlyList<string>> GetTagsAsync(string videoUrl, CancellationToken ct = default);
}
