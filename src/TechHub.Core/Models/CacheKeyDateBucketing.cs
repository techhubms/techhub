namespace TechHub.Core.Models;

/// <summary>
/// Shared helper for cache keys built from "now"-relative date ranges (e.g. lastDays).
/// Rounds timestamps down to a fixed bucket so repeated calls within the same bucket
/// reuse the same cache entry instead of missing on every call because "now" ticks
/// forward every second. 5 minutes matches the shortest entry TTL (search/facets).
/// </summary>
internal static class CacheKeyDateBucketing
{
    private const long BucketSeconds = 300;

    public static long ToBucketedUnixSeconds(this DateTimeOffset value)
    {
        var unixSeconds = value.ToUnixTimeSeconds();
        var remainder = unixSeconds % BucketSeconds;

        if (remainder == 0)
        {
            return unixSeconds;
        }

        return unixSeconds - remainder - (unixSeconds < 0 ? BucketSeconds : 0);
    }
}
