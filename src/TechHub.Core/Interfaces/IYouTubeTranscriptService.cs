using TechHub.Core.Models.ContentProcessing;

namespace TechHub.Core.Interfaces;

/// <summary>
/// Fetches YouTube video closed captions (transcripts).
/// </summary>
public interface IYouTubeTranscriptService
{
    /// <summary>
    /// Fetches the transcript for <paramref name="videoUrl"/>.
    /// Returns a <see cref="TranscriptResult"/> indicating success with text or failure with a reason.
    /// </summary>
    Task<TranscriptResult> GetTranscriptAsync(string videoUrl, CancellationToken ct = default);
}
