namespace TechHub.Web.Services;

/// <summary>
/// Service that provides a base64-encoded favicon data URI to eliminate HTTP requests.
/// The favicon is encoded once at application startup and cached for the lifetime of the app.
/// </summary>
public sealed class FaviconService
{
    public FaviconService(IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(environment);

        var faviconPath = Path.Combine(environment.WebRootPath, "favicon.ico");

        if (File.Exists(faviconPath))
        {
            var faviconBytes = File.ReadAllBytes(faviconPath);
            var base64 = Convert.ToBase64String(faviconBytes);
            DataUri = $"data:image/x-icon;base64,{base64}";
        }
        else
        {
            // Fallback to empty data URI if favicon doesn't exist
            DataUri = "data:image/x-icon;base64,";
        }
    }

    /// <summary>
    /// Gets the base64-encoded favicon as a data URI.
    /// </summary>
    public string DataUri { get; }
}
