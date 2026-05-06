using TechHub.Core.Models;

namespace TechHub.Web.Services;

/// <summary>
/// In-memory cache of hero banner data loaded at application startup.
/// Provides synchronous access to banner data so every section page renders
/// without an extra API round-trip per request.
/// Periodically refreshed and also invalidated by the admin cache-invalidation button.
/// </summary>
public class HeroBannerCache
{
    public HeroBannerData? Data { get; private set; }

    public bool IsInitialized { get; private set; }

    public void Initialize(HeroBannerData? data)
    {
        Data = data;
        IsInitialized = true;
    }

    public void Invalidate()
    {
        Data = null;
        IsInitialized = false;
    }
}
