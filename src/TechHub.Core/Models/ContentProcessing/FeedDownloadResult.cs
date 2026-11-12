namespace TechHub.Core.Models.ContentProcessing;

/// <summary>
/// Result of downloading a feed: the XML on success, or a short description of why the download failed
/// (for example "HTTP 404 (NotFound)" or "Timed out").
/// </summary>
public sealed class FeedDownloadResult
{
    /// <summary>The downloaded feed XML, or <see langword="null"/> when the download failed.</summary>
    public string? Xml { get; init; }

    /// <summary>Why the download failed, or <see langword="null"/> when it succeeded.</summary>
    public string? Error { get; init; }

    /// <summary>Whether the download failed.</summary>
    public bool Failed => Xml is null;

    /// <summary>Creates a successful result with the downloaded XML.</summary>
    public static FeedDownloadResult Success(string xml) => new() { Xml = xml };

    /// <summary>Creates a failed result with the reason the download failed.</summary>
    public static FeedDownloadResult Failure(string error) => new() { Error = error };
}
