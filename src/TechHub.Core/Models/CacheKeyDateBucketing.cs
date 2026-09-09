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

    public static long ToBucketedUnixSeconds(this DateTimeOffset value) =>
        value.ToUnixTimeSeconds() / BucketSeconds * BucketSeconds;
}
