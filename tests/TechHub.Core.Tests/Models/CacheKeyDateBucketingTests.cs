using FluentAssertions;
using TechHub.Core.Models;

namespace TechHub.Core.Tests.Models;

/// <summary>
/// Tests the 5-minute date bucketing that cache keys apply to "now"-relative date filters.
/// The bucketing helper is internal, so it is verified through the public <see cref="SearchRequest.GetCacheKey"/>.
/// </summary>
public class CacheKeyDateBucketingTests
{
    // 1_700_000_100 is an exact multiple of 300 seconds (a bucket boundary)
    private const long BucketStart = 1_700_000_100;

    private static SearchRequest CreateRequest(DateTimeOffset? dateFrom = null, DateTimeOffset? dateTo = null) =>
        new(
            take: 10,
            sections: ["all"],
            collections: ["all"],
            tags: [],
            dateFrom: dateFrom,
            dateTo: dateTo);

    [Theory]
    [InlineData(0, BucketStart)]
    [InlineData(1, BucketStart)]
    [InlineData(299, BucketStart)]
    [InlineData(300, BucketStart + 300)]
    [InlineData(301, BucketStart + 300)]
    public void GetCacheKey_DateFrom_RoundsDownToFiveMinuteBucket(long offsetSeconds, long expectedBucket)
    {
        // Arrange
        var request = CreateRequest(dateFrom: DateTimeOffset.FromUnixTimeSeconds(BucketStart + offsetSeconds));

        // Act
        var key = request.GetCacheKey();

        // Assert
        key.Should().Contain($"|df:{expectedBucket}|take:");
    }

    [Fact]
    public void GetCacheKey_DateTo_RoundsDownToFiveMinuteBucket()
    {
        // Arrange
        var request = CreateRequest(dateTo: DateTimeOffset.FromUnixTimeSeconds(BucketStart + 150));

        // Act
        var key = request.GetCacheKey();

        // Assert
        key.Should().Contain($"|dt:{BucketStart}|take:");
        key.Should().NotContain("|df:");
    }

    [Fact]
    public void GetCacheKey_DatesWithinSameBucket_ProduceSameKey()
    {
        // Arrange
        var first = CreateRequest(dateFrom: DateTimeOffset.FromUnixTimeSeconds(BucketStart + 10));
        var second = CreateRequest(dateFrom: DateTimeOffset.FromUnixTimeSeconds(BucketStart + 290));

        // Act & Assert
        first.GetCacheKey().Should().Be(second.GetCacheKey());
    }

    [Fact]
    public void GetCacheKey_DatesInDifferentBuckets_ProduceDifferentKeys()
    {
        // Arrange
        var first = CreateRequest(dateFrom: DateTimeOffset.FromUnixTimeSeconds(BucketStart + 299));
        var second = CreateRequest(dateFrom: DateTimeOffset.FromUnixTimeSeconds(BucketStart + 300));

        // Act & Assert
        first.GetCacheKey().Should().NotBe(second.GetCacheKey());
    }

    [Theory]
    [InlineData(-1, -300)]
    [InlineData(-300, -300)]
    [InlineData(-301, -600)]
    public void GetCacheKey_DateBeforeUnixEpoch_RoundsTowardsEarlierBucket(long unixSeconds, long expectedBucket)
    {
        // Arrange
        var request = CreateRequest(dateFrom: DateTimeOffset.FromUnixTimeSeconds(unixSeconds));

        // Act
        var key = request.GetCacheKey();

        // Assert
        key.Should().Contain($"|df:{expectedBucket}|take:");
    }

    [Fact]
    public void GetCacheKey_NoDates_OmitsDateSegments()
    {
        // Arrange
        var request = CreateRequest();

        // Act
        var key = request.GetCacheKey();

        // Assert
        key.Should().NotContain("|df:").And.NotContain("|dt:");
    }
}
