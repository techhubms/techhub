namespace TechHub.Core;

/// <summary>
/// Thrown when an operation attempts to use a YouTube URL that is already owned by
/// a different content item in the processed_urls table.
/// </summary>
public sealed class ProcessedUrlConflictException : Exception
{
    public ProcessedUrlConflictException()
        : base("The YouTube URL is already owned by another content item in processed_urls.")
    {
    }

    public ProcessedUrlConflictException(string message)
        : base(message)
    {
    }

    public ProcessedUrlConflictException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
