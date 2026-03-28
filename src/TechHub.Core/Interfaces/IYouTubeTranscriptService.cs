namespace TechHub.Core.Interfaces;

/// <summary>
/// Fetches YouTube video closed captions (transcripts).
/// </summary>
public interface IYouTubeTranscriptService
{
    /// <summary>
    /// Fetches the transcript for <paramref name="videoUrl"/>.
    /// Returns <see langword="null"/> if no captions are available or on failure.
    /// </summary>
    Task<string?> GetTranscriptAsync(string videoUrl, CancellationToken ct = default);
}
